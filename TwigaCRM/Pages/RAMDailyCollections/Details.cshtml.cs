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

namespace TwigaCRM.Pages.RAMDailyCollections
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

        [BindProperty]
        public RAMDailyCollectionReport RAMDailyCollectionReport { get; set; }
        public List<RAMDailyCollection> RAMDailyCollections { get; set; }
        public string ApprovalStatus { get; set; }
        public AppUser AppUser { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Permission> Permissions { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_Daily_Collection_details");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);

            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            RAMDailyCollectionReport = await _context.RAMDailyCollectionReport
                .Include(d => d.RAM)
                .Include(d => d.RAM.AppRole)
                .Include(d => d.RAM.Town)
                .Include(d => d.RAM.Town.Region).FirstOrDefaultAsync(m => m.Id == id);
            RAMDailyCollections = await _context.RAMDailyCollection.Where(d => d.RAMDailyCollectionReportId == RAMDailyCollectionReport.Id)
                .Include(d => d.MainDistributor)
                .Include(d => d.RAMDailyCollectionReport).ToListAsync();
            Customers = await _context.Customer.ToListAsync();
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).ToListAsync();

            ViewData["MainDistributors"] = _context.Customer.Include(t => t.Town)
                                            .Include(t => t.Town.Region)
                                                .Where(t => t.Town.Region.Id == AppUser.Town.Region.Id)
                                                .Where(t => t.CustomerType == "Main Distributor").Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.SiteName + " - " + a.ContactPersonName
                                            }).ToList();
            if (RAMDailyCollectionReport == null)
            {
                return NotFound();
            }
            return Page();
        }
        [BindProperty]
        public RAMDailyCollection RAMDailyCollection { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_daily_collection");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Details", new { id = RAMDailyCollection.RAMDailyCollectionReportId });
            }
            _context.RAMDailyCollection.Add(RAMDailyCollection);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Daily Collection Recorded!");

            return RedirectToPage("./Details", new { id = RAMDailyCollection.RAMDailyCollectionReportId });
        }
    }
}
