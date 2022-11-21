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

namespace TwigaCRM.Pages.RAMPlans
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

        public IList<RAMPlan> RAMPlans { get;set; }
        public AppUser AppUser { get; set; }
        public bool IsPermitted { get; private set; }
        public List<Permission> Permissions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_RAM_route_plans");
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
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).ToListAsync();
            RAMPlans = await _context.RAMPlan
                .Include(r => r.RAM).OrderByDescending(s => s.Id).ToListAsync();
            return Page();
        }

        [BindProperty]
        public RAMPlan RAMPlan { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "create_RAM_route_plan");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./RAMPlans");
            }
            var UserId = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            var CurrentPlan = await _context.RAMPlan.Where(d => d.RAMId == UserId && d.StartDate.Date == RAMPlan.StartDate.Date).FirstOrDefaultAsync();
            if (CurrentPlan != null)
            {
                _toastNotification.Warning("Route Plan Already Exists!");
                return RedirectToPage("./Details", new { id = CurrentPlan.Id });
            }
            if (RAMPlan.StartDate.DayOfWeek.ToString() != "Monday")
            {
                _toastNotification.Information("Route RAMPlan Start Date Must Be A Monday!");
                return RedirectToPage("./RAMPlans");
            }
            RAMPlan.IsSubmitted = false;
            RAMPlan.ApprovalStatus = "Pending";
            _context.RAMPlan.Add(RAMPlan);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("RAM Route Plan Created!");

            CurrentPlan = await _context.RAMPlan
               .Where(s => s.RAMId == RAMPlan.RAMId && s.StartDate.Date == RAMPlan.StartDate.Date).FirstOrDefaultAsync();

            return RedirectToPage("./Details", new { id = CurrentPlan.Id });
        }
    }
}
