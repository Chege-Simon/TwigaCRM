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
using NToastNotify;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.DailyMovementReports
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
        public TwigaCRM.Models.DailyMovementReport DailyMovementReport { get; set; }
        public List<DailyMovement> DailyMovements { get; set; }
        public AppUser AppUser { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Permission> Permissions { get; set; }
        [BindProperty]
        public int DailyMovementReportId { get; set; }
        [BindProperty]
        public string FOAstatus { get; set; }
        [BindProperty]
        public string TLstatus { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_DMR_details");
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
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            DailyMovementReport = await _context.DailyMovementReport
                .Include(d => d.SalesPerson)
                .Include(d => d.SalesPerson.AppRole)
                .Include(d => d.SalesPerson.Town)
                .Include(d => d.SalesPerson.Town.Region).FirstOrDefaultAsync(m => m.Id == id);
            DailyMovements = await _context.DailyMovement.Where(d => d.DailyMovementReportId == DailyMovementReport.Id)
                .Include(d => d.MainDistributor)
                .Include(d => d.Product)
                .Include(d => d.DailyMovementReport).OrderByDescending(s => s.Id).ToListAsync();
            Customers = await _context.Customer.OrderByDescending(s => s.Id).ToListAsync();
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).OrderByDescending(s => s.Id).ToListAsync();
            FOAstatus = DailyMovementReport.FOAstatus;
            TLstatus = DailyMovementReport.TLstatus;

            if (DailyMovementReport == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostFOAApproveAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "foa_approve_DMR");
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
            DailyMovementReport = await _context.DailyMovementReport.FirstOrDefaultAsync(d => d.Id == id);

            DailyMovementReport.IsSubmitted = FOAstatus != "Rejected" ? true : false;
            DailyMovementReport.TLstatus = DailyMovementReport.FOAstatus != FOAstatus?"Pending" : DailyMovementReport.TLstatus;
            DailyMovementReport.FOAstatus = FOAstatus;
            _context.Attach(DailyMovementReport).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("D.M.R Approval Changed!");

            return RedirectToPage("./Details", new { id});
        }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostTLApproveAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "tl_approve_DMR");
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

            DailyMovementReport = await _context.DailyMovementReport.FirstOrDefaultAsync(d => d.Id == id);

            DailyMovementReport.IsSubmitted = TLstatus != "Rejected" ? true : false;
            DailyMovementReport.TLstatus = TLstatus;
            _context.Attach(DailyMovementReport).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("D.M.R Approval Changed!");

            return RedirectToPage("./Details", new { id });
        }
    }
}
