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

namespace TwigaCRM.Pages.DailyMovements
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

        public IList<DailyMovement> DailyMovements { get;set; }
        public AppUser AppUser { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Permission> Permissions { get; set; }
        public bool Approver { get; set; }
        [BindProperty]
        public int DailyMovementReportId { get; set; }
        public IList<DailyMovementReport> DailyMovementReports { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_daily_movements");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            DailyMovementReportId = id;
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);

            DailyMovementReports = await _context.DailyMovementReport
                .Include(d => d.SalesPerson).Where(d => d.SalesPersonId == Id).OrderByDescending(s => s.Id).ToListAsync();
            return Page();

        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        [BindProperty]
        public DailyMovementReport DailyMovementReport { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "create_DMR");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./DailyMovements");
            }
            var UserId = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            var CurrentDailyMovementReport = await _context.DailyMovementReport.Where(d => d.SalesPersonId == UserId && d.SalesDate.Date == DailyMovementReport.SalesDate.Date).FirstOrDefaultAsync();
            if (CurrentDailyMovementReport != null)
            {
                _toastNotification.Warning("D.M.R Already Exists!");
                return RedirectToPage("./Details", new { id = CurrentDailyMovementReport.Id });
            }
            DailyMovementReport.IsSubmitted = false;
            DailyMovementReport.FOAstatus = "Pending";
            DailyMovementReport.TLstatus = "Pending";
            _context.DailyMovementReport.Add(DailyMovementReport);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Daily Movement Report Created!");
            CurrentDailyMovementReport = await _context.DailyMovementReport.Where(d => d.SalesPersonId == UserId && d.SalesDate.Date == DailyMovementReport.SalesDate.Date).FirstOrDefaultAsync();

            return RedirectToPage("./Details", new {id = CurrentDailyMovementReport.Id });
        }
    }
}
