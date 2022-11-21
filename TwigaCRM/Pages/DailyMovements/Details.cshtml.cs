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

namespace TwigaCRM.Pages.DailyMovements
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

        public TwigaCRM.Models.DailyMovementReport DailyMovementReport { get; set; }
        public List<DailyMovement> DailyMovements { get; set; }
        public AppUser AppUser { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Permission> Permissions { get; set; }
        [BindProperty]
        public int DailyMovementReportId { get; set; }
        public bool IsPermitted { get; private set; }
        public List<Customer> AllCustomers { get; set;}

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_daily_movement_details");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }
            DailyMovementReportId = (int)id;
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.UserBusinessLines)
                .Include(u => u.UserSectors)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            AllCustomers = await _context.Customer.ToListAsync();
            DailyMovementReport = await _context.DailyMovementReport
                .Include(d => d.SalesPerson)
                .Include(d => d.SalesPerson.AppRole)
                .Include(d => d.SalesPerson.Town)
                .Include(d => d.SalesPerson.Town.Region).FirstOrDefaultAsync(m => m.Id == id);
            DailyMovements = await _context.DailyMovement.Where(d => d.DailyMovementReportId == DailyMovementReport.Id)
                .Include(d => d.MainDistributor)
                .Include(d => d.Product)
                .Include(d => d.DailyMovementReport).OrderByDescending(s => s.Id).ToListAsync(); 
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).ToListAsync();
             if (AppUser.Town != null)
            {
                ViewData["MainDistributors"] = _context.Customer.Include(t => t.Town).Include(t => t.Town.Region)
                                                .Where(t => t.Town.Region.Id == AppUser.Town.Region.Id)
                                                .Where(t => t.CustomerType == "Main Distributor").Select(a =>
                                                new SelectListItem
                                                {
                                                    Value = a.Id.ToString(),
                                                    Text = a.SiteName + " - " + a.ContactPersonName
                                                }).ToList();
            }
            else
            {
                ViewData["MainDistributors"] = _context.Customer.Include(t => t.Town)
                                                .Include(t => t.Town.Region)
                                                    .Where(t => t.CustomerType == "Main Distributor").Select(a =>
                                                new SelectListItem
                                                {
                                                    Value = a.Id.ToString(),
                                                    Text = a.SiteName + " - " + a.ContactPersonName
                                                }).ToList();
            }
            List<Product> selectableProducts = new List<Product>();
            List<Product> Products = await _context.Product.Include(p => p.BusinessLine).ToListAsync();
            foreach (var businessline in AppUser.UserBusinessLines)
            {
                foreach (var product in Products)
                {
                    if (product.BusinessLineId == businessline.BusinessLineId)
                    {
                        selectableProducts.Add(product);
                    }
                }
            }
            ViewData["Products"] = selectableProducts.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name + " (" + a.Description + ") - " + " (" + a.Code + ") - " + a.BusinessLine.NormalizedName
                                            }).ToList();
            ViewData["Stokists"] = _context.Customer.Include(t => t.Town).Include(t => t.Town.Region).Where(t => t.Town.Region.Id == AppUser.Town.Region.Id)
                                                .Where(t => t.CustomerType == "Stockist").Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.SiteName + " - " + a.ContactPersonName
                                            }).ToList();
            Customers = new List<Customer>();
            List<CustomerSector> CustomerSectors = await _context.CustomerSector.Include(c => c.Sector).ToListAsync();
            List<Customer> SelectableCustomers = new List<Customer>();
            foreach(var customersector in CustomerSectors)
            {
                foreach (var usersector in AppUser.UserSectors)
                {
                    if(usersector.SectorId == customersector.SectorId)
                    {
                        Customers.Add(_context.Customer.Include(t => t.Town).Include(t => t.Town.Region).Include(t => t.CustomerSectors).FirstOrDefault(c => c.Id == customersector.CustomerId));
                    }
                }
            }
            foreach (var customer in Customers)
            {
                if (customer.CustomerType == "Corporate" || (customer.CustomerType == "Farmer" && customer.FarmerType == "M.S.F"))
                {
                    SelectableCustomers.Add(customer);
                }
            }
            ViewData["MSFOrCorporates"] = Customers.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.SiteName + " - " + a.ContactPersonName + " @ " + a.Town.Name + " - " + a.Town.Region.Name
                                            }).ToList();

            if (DailyMovementReport == null)
            {
                return NotFound();
            }
            return Page();
        }
        [BindProperty]
        public DailyMovement DailyMovement { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_daily_movement");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Details", new {id = DailyMovement.DailyMovementReportId});
            }
            Product currentProduct = await _context.Product.FirstOrDefaultAsync(p => p.Id == DailyMovement.ProductId);
            var cost = (currentProduct.Price * currentProduct.PackagingSize) * (DailyMovement.Quantity / currentProduct.PackagingSize);
            DailyMovement.TotalAmount = cost;
            _context.DailyMovement.Add(DailyMovement);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Daily Movement Recorded!");

            return RedirectToPage("./Details", new { id = DailyMovement.DailyMovementReportId});
        }
    }
}
