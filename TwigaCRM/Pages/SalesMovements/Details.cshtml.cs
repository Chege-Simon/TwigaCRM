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
using TwigaCRM.Migrations;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.SalesMovements
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public DetailsModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }
        public List<Permission> Permissions { get; set; }
        public SalesMovement SalesMovement { get; set; }
        public IList<Target> Targets { get; set; }
        [BindProperty]
        public int SalesMovementId { get; set; }
        public AppUser AppUser { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_MT_details");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            SalesMovementId = (int)id;
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            SalesMovement = await _context.SalesMovement
                .Include(s => s.FinancialYear)
                .Include(s => s.SalesPerson)
                .Include(s => s.SalesPerson.UserSectors)
                .Include(s => s.SalesPerson.UserBusinessLines)
                .Include(s => s.SalesPerson.Town)
                .Include(u => u.SalesPerson.Town.Region)
                .Include(s => s.SalesPerson.AppRole).FirstOrDefaultAsync(m => m.Id == id);

            Targets = await _context.Target
                .Include(t => t.CropAndAnimal)
                .Include(t => t.Product)
                .Include(t => t.SalesMovement)
                .Where(t => t.SalesMovementId == SalesMovement.Id).OrderByDescending(s => s.Id).ToListAsync();
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).ToListAsync();
            if (SalesMovement == null)
            {
                return NotFound();
            }
            List<CropAndAnimal> AllCropAndAnimals = _context.CropAndAnimal
                .Include(t => t.ProductsCropsAndAnimals)
                .Include(t => t.Sector).ToList();
            List<CropAndAnimal> SelectableCropAndAnimals = new() { };
            foreach (var cropandanimal in AllCropAndAnimals)
            {
                foreach (var sector in SalesMovement.SalesPerson.UserSectors)
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
                foreach (var businessline in SalesMovement.SalesPerson.UserBusinessLines)
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
                                                Text = a.Name + " (" + a.Code + ") - " + a.Description+ " - " +a.BusinessLine.NormalizedName
                                            }).ToList();
            List<Customer> AllCustomers = await _context.Customer.Include(c => c.CustomerSectors).Where(c => c.CustomerType == "Main Distributor" || (c.CustomerType == "Farmer" && c.FarmerType == "M.S.F")).ToListAsync();
            List<Customer> SelectableCustomers = new List<Customer>() { };
            foreach (var customer in AllCustomers)
            {
                foreach (var sector in SalesMovement.SalesPerson.UserSectors)
                {
                    foreach (var customersector in customer.CustomerSectors)
                    {
                        if (customersector.SectorId == sector.SectorId)
                        {
                            SelectableCustomers.Add(customer);
                        }
                    }
                }
            }
            ViewData["Customers"] = SelectableCustomers.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.SiteName
                                            }).ToList();
            return Page();
        }
        [BindProperty]
        public Target Target { get; set; }
        [BindProperty]
        public string TLstatus { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_target");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Details", new {id = Target.SalesMovementId});
            }

            Product CurrentProduct = await _context.Product.FirstOrDefaultAsync(p => p.Id == Target.ProductId);
            ProductCropAndAnimal productCropAndAnimal = await _context.ProductCropAndAnimal
                .Where(p => p.CropAndAnimalId == Target.CropAndAnimalId && p.ProductId == Target.ProductId).FirstOrDefaultAsync();
            if (productCropAndAnimal == null)
            {
                _toastNotification.Warning("Product And Animal / Crop Miss Match!");
                return RedirectToPage("./Details", new { id = Target.SalesMovementId });
            }
            if (Target.CropAndAnimalQuantity <= 0)
            {
                Customer CurrentCustomer = await _context.Customer.FirstOrDefaultAsync(c => c.Id == Target.TargetCustomerId);
                try
                {
                    Target.BusinessPotential = ((int.Parse(CurrentCustomer.FarmSize) * (decimal)productCropAndAnimal.NoOfTimeApplied) / productCropAndAnimal.Servings) * CurrentProduct.PackagingSize;
                }
                catch (Exception)
                {
                    _toastNotification.Warning("Farm / Corparate missing farm size!");
                    return RedirectToPage("./Details", new { id = Target.SalesMovementId });
                }
            }
            else
            {
                Target.BusinessPotential = ((Target.CropAndAnimalQuantity * (decimal)productCropAndAnimal.NoOfTimeApplied) / productCropAndAnimal.Servings) * CurrentProduct.PackagingSize;
            }
            //Target.Volume = (Target.BusinessPotential * Target.MarketShare) / 100;
            Target.MarketShare = (Target.Volume * 100) / Target.BusinessPotential;
            Target.Value = (Target.Volume / CurrentProduct.PackagingSize) * (CurrentProduct.Price * CurrentProduct.PackagingSize);
            _context.Target.Add(Target);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Target Recorded!");

            return RedirectToPage("./Details", new { id = Target.SalesMovementId });
        }

        public async Task<IActionResult> OnPostTLApproveAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "tl_approve_MT");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Details", new { id });
            }

            SalesMovement = await _context.SalesMovement.FirstOrDefaultAsync(d => d.Id == id);

            SalesMovement.IsSubmitted = TLstatus != "Rejected" ? true : false;
            SalesMovement.TLstatus = TLstatus;
            _context.Attach(SalesMovement).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("M.T Status Changed!");

            return RedirectToPage("./Details", new { id });
        }
        [BindProperty]
        public string Remarks { get; set; }
        public async Task<IActionResult> OnPostAddRemarksAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_MT_remarks");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Details", new { id });
            }

            SalesMovement = await _context.SalesMovement.FirstOrDefaultAsync(d => d.Id == id);
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);

            SalesMovement.Remarks = SalesMovement.Remarks + System.Environment.NewLine + AppUser.FirstName+" " +AppUser.LastName+ " ( " + AppUser.AppRole.Title + ") : " + Remarks;
            _context.Attach(SalesMovement).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Remarks Added!");

            return RedirectToPage("./Details", new { id });
        }
    }
}
