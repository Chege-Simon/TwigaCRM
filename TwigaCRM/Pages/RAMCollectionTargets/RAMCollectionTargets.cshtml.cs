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

namespace TwigaCRM.Pages.RAMCollectionTargets
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

        public IList<RAMCollectionTarget> RAMCollectionTargets { get;set; }
        public bool IsPermitted { get; private set; }
        public AppUser AppUser { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_RAM_collection_targets");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);

            RAMCollectionTargets = await _context.RAMCollectionTarget
                .Include(r => r.FinancialYear)
                .Include(r => r.RAM).ToListAsync();

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
        public RAMCollectionTarget RAMCollectionTarget { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "create_RAM_collection_target");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./RAMCollectionTargets");
            }
            RAMCollectionTarget NewRAMCollectionTarget = await _context.RAMCollectionTarget
               .Where(s => s.RAMId == RAMCollectionTarget.RAMId && s.Month == RAMCollectionTarget.Month && s.FinancialYearId == RAMCollectionTarget.FinancialYearId).FirstOrDefaultAsync();
            if (NewRAMCollectionTarget != null)
            {
                _toastNotification.Warning("Collections Target Already Exists!!");
                return RedirectToPage("./Details", new { id = NewRAMCollectionTarget.Id });
            }
            RAMCollectionTarget.IsSubmitted = false;
            RAMCollectionTarget.ApprovalStatus = "Pending";
            _context.RAMCollectionTarget.Add(RAMCollectionTarget);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("RAM Collection Target Created!");
            RAMCollectionTarget CurrentRAMCollectionTarget = await _context.RAMCollectionTarget
               .Where(s => s.RAM == RAMCollectionTarget.RAM && s.Month == RAMCollectionTarget.Month && s.FinancialYearId == RAMCollectionTarget.FinancialYearId).FirstOrDefaultAsync();
            return RedirectToPage("./Details", new { id = CurrentRAMCollectionTarget.Id });
        }
    }
}
