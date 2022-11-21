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

namespace TwigaCRM.Pages.RAMSaleTargets
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

        public IList<RAMSaleTarget> RAMSaleTargets { get;set; }
        public bool IsPermitted { get; private set; }
        public AppUser AppUser { get; set; }
        public List<Permission> Permissions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_RAM_STs");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);

            RAMSaleTargets = await _context.RAMSaleTarget
                .Include(r => r.FinancialYear)
                .Include(r => r.RAM).OrderByDescending(s => s.Id).ToListAsync();
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).ToListAsync();

            ViewData["FinancialYears"] = _context.FinancialYear.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.StartDate.Year + " - " + a.EndDate.Year
                                            }).ToList();
            ViewData["RAMs"] = _userManager.Users.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.FirstName + " " + a.LastName
                                            }).ToList();

            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            return Page();
        }
        [BindProperty]
        public RAMSaleTarget RAMSaleTarget { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "create_RAM_ST");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./RAMSaleTargets");
            }
            RAMSaleTarget NewRAMSaleTarget = await _context.RAMSaleTarget
               .Where(s => s.RAMId == RAMSaleTarget.RAMId && s.Month == RAMSaleTarget.Month && s.FinancialYearId == RAMSaleTarget.FinancialYearId).FirstOrDefaultAsync();
            if (NewRAMSaleTarget != null)
            {
                _toastNotification.Warning("Sale Target Already Exists!!");
                return RedirectToPage("./Details", new { id = NewRAMSaleTarget.Id });
            }
            RAMSaleTarget.IsSubmitted = false;
            RAMSaleTarget.ApprovalStatus = "Pending";
            _context.RAMSaleTarget.Add(RAMSaleTarget);
            await _context.SaveChangesAsync();
            RAMSaleTarget CurrentRAMSaleTarget = await _context.RAMSaleTarget
               .Where(s => s.RAM == RAMSaleTarget.RAM && s.Month == RAMSaleTarget.Month && s.FinancialYearId == RAMSaleTarget.FinancialYearId).FirstOrDefaultAsync();
            return RedirectToPage("./Details", new { id = CurrentRAMSaleTarget.Id });
        }
    }
}
