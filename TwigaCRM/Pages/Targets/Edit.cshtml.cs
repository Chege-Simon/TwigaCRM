using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.Targets
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;
        public AppUser AppUser { get; set; }
        public SalesMovement SalesMovement { get; set; }

        public EditModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        [BindProperty]
        public Target Target { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_target");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);

            Target = await _context.Target
                .Include(t => t.CropAndAnimal)
                .Include(t => t.Product)
                .Include(t => t.SalesMovement)
                .Include(t => t.SalesMovement.SalesPerson)
                .Include(t => t.SalesMovement.SalesPerson.UserSectors)
                .Include(t => t.SalesMovement.SalesPerson.UserBusinessLines)
                .Include(t => t.SalesMovement).FirstOrDefaultAsync(m => m.Id == id);
            if (Target == null)
            {
                return NotFound();
            }
            List<CropAndAnimal> AllCropAndAnimals = _context.CropAndAnimal
                .Include(t => t.ProductsCropsAndAnimals)
                .Include(t => t.Sector).ToList();
            List<CropAndAnimal> SelectableCropAndAnimals = new() { };
            foreach (var cropandanimal in AllCropAndAnimals)
            {
                foreach (var sector in Target.SalesMovement.SalesPerson.UserSectors)
                {
                    if (cropandanimal.SectorId == sector.SectorId)
                    {
                        SelectableCropAndAnimals.Add(cropandanimal);
                    }
                }
            }
            ViewData["CropAndAnimals"] = SelectableCropAndAnimals.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name + " - " + a.Sector.NormalizedName + " Sector"
                                            }).ToList();
            List<Product> AllProducts = _context.Product
                .Include(t => t.BusinessLine)
                .Include(t => t.ProductsCropsAndAnimals).ToList();
            List<Product> SelectableProducts = new() { };
            foreach (var product in AllProducts)
            {
                foreach (var businessline in Target.SalesMovement.SalesPerson.UserBusinessLines)
                {
                    if (product.BusinessLineId == businessline.BusinessLineId)
                    {
                        SelectableProducts.Add(product);
                    }
                }
            }
            ViewData["Products"] = SelectableProducts.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name + " (" + a.Code + ") - " + a.BusinessLine.NormalizedName
                                            }).ToList();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_target");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("../SalesMovements/Details", new { id = Target.SalesMovementId });
            }
            Product CurrentProduct = await _context.Product.FirstOrDefaultAsync(p => p.Id == Target.ProductId);
            ProductCropAndAnimal productCropAndAnimal = await _context.ProductCropAndAnimal
                .Where(p => p.CropAndAnimalId == Target.CropAndAnimalId && p.ProductId == Target.ProductId).FirstOrDefaultAsync();
            if (productCropAndAnimal == null)
            {
                _toastNotification.Warning("Product And Animal / Crop Miss Match!");
                return RedirectToPage("../SalesMovements/Details", new { id = Target.SalesMovementId });
            }

            if (Target.SalesMovement.CreatorId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../SalesMovements/SalesMovements");
            }
            Target.BusinessPotential = Target.CropAndAnimalQuantity * productCropAndAnimal.Servings;
            Target.Volume = (Target.BusinessPotential * Target.MarketShare) / 100;
            Target.Value = Target.Volume * CurrentProduct.Price;
            _context.Attach(Target).State = EntityState.Modified;

            try
            {
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("Target Edited!");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TargetExists(Target.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../SalesMovements/Details", new { id = Target.SalesMovementId });
        }

        private bool TargetExists(int id)
        {
            return _context.Target.Any(e => e.Id == id);
        }
    }
}
