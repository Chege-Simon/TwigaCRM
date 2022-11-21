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
using Microsoft.EntityFrameworkCore;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.RAMDailyCollections
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;


        public IndexModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        public IList<RAMDailyCollection> RAMDailyCollections { get;set; }
        public AppUser AppUser { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Permission> Permissions { get; set; }
        public bool Approver { get; set; }
        [BindProperty]
        public int RAMDailyCollectionReportId { get; set; }
        public IList<RAMDailyCollectionReport> RAMDailyCollectionReports { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {

            IsPermitted = _checkPermissions.CheckPermission(User, "view_daily_collections");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            //RAMDailyCollectionReportId = id;
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);

            RAMDailyCollectionReports = await _context.RAMDailyCollectionReport
                .Include(d => d.RAM).Where(d => d.RAMId == Id).ToListAsync();
            return Page();

        }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        [BindProperty]
        public RAMDailyCollectionReport RAMDailyCollectionReport { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "create_DCR");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./RAMDailyCollections");
            }
            var UserId = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            var CurrentRAMDailyCollectionReport = await _context.RAMDailyCollectionReport.Where(d => d.RAMId == UserId && d.SalesDate.Date == RAMDailyCollectionReport.SalesDate.Date).FirstOrDefaultAsync();
            if (CurrentRAMDailyCollectionReport != null)
            {
                _toastNotification.Warning("D.C.R Already Exists!");
                return RedirectToPage("./Details", new { id = CurrentRAMDailyCollectionReport.Id });
            }
            RAMDailyCollectionReport.IsSubmitted = false;
            RAMDailyCollectionReport.ApprovalStatus = "Pending";
            _context.RAMDailyCollectionReport.Add(RAMDailyCollectionReport);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Daily Collection Report Created!");
            CurrentRAMDailyCollectionReport = await _context.RAMDailyCollectionReport.Where(d => d.RAMId == UserId && d.SalesDate.Date == RAMDailyCollectionReport.SalesDate.Date).FirstOrDefaultAsync();

            return RedirectToPage("./Details", new { id = CurrentRAMDailyCollectionReport.Id });
        }
    }
}
