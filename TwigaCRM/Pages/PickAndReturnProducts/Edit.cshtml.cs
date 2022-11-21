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
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.PickAndReturnProducts
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly CheckPermissionsService _checkPermissions;
        private readonly INotyfService _toastNotification;
        private readonly UserManager<AppUser> _userManager;

        public EditModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _checkPermissions = checkPermissions;
            _toastNotification = toastNotification;
            _userManager = userManager;
        }

        [BindProperty]
        public PickAndReturnProduct PickAndReturnProduct { get; set; }
        public AppUser AppUser { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_pick_and_return_product");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            PickAndReturnProduct = await _context.PickAndReturnProduct
                .Include(p => p.Product).FirstOrDefaultAsync(m => m.Id == id);

            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            if (PickAndReturnProduct == null)
            {
                return NotFound();
            }
            ViewData["Products"] = _context.Product.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name + " (" + a.Description + ") - " + " (" + a.Code + ")"
                                            }).ToList();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_pick_and_return_product");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("../PickAndReturnForms/Details", new { id = PickAndReturnProduct.PickAndReturnFormId });
            }

            PickAndReturnForm PickAndReturnForm = await _context.PickAndReturnForm
                .Include(p => p.MainDistributor).FirstOrDefaultAsync(m => m.Id == PickAndReturnProduct.PickAndReturnFormId);
            if (PickAndReturnForm.VSAId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../PickAndReturnForms/PickAndReturnForms");
            }

            Product currentProduct = await _context.Product.FirstOrDefaultAsync(p => p.Id == PickAndReturnProduct.ProductId);
            var pickedcost = (currentProduct.Price * currentProduct.PackagingSize) * (PickAndReturnProduct.PickedQuantity / currentProduct.PackagingSize);
            var returnedcost = (currentProduct.Price * currentProduct.PackagingSize) * (PickAndReturnProduct.ReturnedQuantity / currentProduct.PackagingSize);
            var pickedvolume = currentProduct.PackagingSize * PickAndReturnProduct.PickedQuantity;
            var returnedvolume = currentProduct.PackagingSize * PickAndReturnProduct.ReturnedQuantity;
            PickAndReturnProduct.PickedVolume = pickedvolume;
            PickAndReturnProduct.PickedValue = pickedcost;
            PickAndReturnProduct.ReturnedVolume = returnedvolume;
            PickAndReturnProduct.ReturnedValue = returnedcost;
            _context.Attach(PickAndReturnProduct).State = EntityState.Modified;

            try
            {
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("Pick And Return Product Edited!");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PickAndReturnProductExists(PickAndReturnProduct.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../PickAndReturnForms/Details", new { id = PickAndReturnProduct.PickAndReturnFormId });
        }

        private bool PickAndReturnProductExists(int id)
        {
            return _context.PickAndReturnProduct.Any(e => e.Id == id);
        }
    }
}
