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
using TwigaCRM.Migrations;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.RAMDailySaleReports
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
        public RAMDailySaleReport RAMDailySaleReport { get; set; }
        public List<RAMDailySale> RAMDailySales { get; set; }
        public AppUser AppUser { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Permission> Permissions { get; set; }
        [BindProperty]
        public int RAMDailySaleReportId { get; set; }
        [BindProperty]
        public string ApprovalStatus { get; set; }
        public bool IsPermitted { get; private set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_DSR_details");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }
            RAMDailySaleReportId = (int)id;
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            RAMDailySaleReport = await _context.RAMDailySaleReport
                .Include(d => d.RAM)
                .Include(d => d.RAM.AppRole)
                .Include(d => d.RAM.Town)
                .Include(d => d.RAM.Town.Region).FirstOrDefaultAsync(m => m.Id == id);
            RAMDailySales = await _context.RAMDailySale.Where(d => d.RAMDailySaleReportId == RAMDailySaleReport.Id)
                .Include(d => d.MainDistributor)
                .Include(d => d.RAMDailySaleReport).OrderByDescending(s => s.Id).ToListAsync();
            Customers = await _context.Customer.ToListAsync();
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).ToListAsync();
            ApprovalStatus = RAMDailySaleReport.ApprovalStatus;

            if (RAMDailySaleReport == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "approve_DSR");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./RAMDailySales");
            }
            RAMDailySaleReport = await _context.RAMDailySaleReport.FirstOrDefaultAsync(d => d.Id == id);

            RAMDailySaleReport.IsSubmitted = ApprovalStatus != "Rejected" ? true : false;
            RAMDailySaleReport.ApprovalStatus = ApprovalStatus;
            _context.Attach(RAMDailySaleReport).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("D.C.R Approval Changed!");

            return RedirectToPage("./Details", new { id });
        }
    }
}
