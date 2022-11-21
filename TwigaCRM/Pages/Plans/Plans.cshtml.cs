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

namespace TwigaCRM.Pages.Plans
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

        public IList<Plan> Plans { get;set; }
        public AppUser AppUser { get; set; }
        public bool IsPermitted { get; private set; }
        public List<Permission> Permissions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_route_plans");
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

            Plans = await _context.Plan
                .Include(p => p.SalesPerson).OrderByDescending(s => s.Id).ToListAsync();
            return Page();
        }
        [BindProperty]
        public Plan Plan { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "create_route_plan");
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
            var CurrentPlan = await _context.Plan.Where(d => d.SalesPersonId == UserId && d.StartDate.Date == Plan.StartDate.Date).FirstOrDefaultAsync();
            if (CurrentPlan != null)
            {
                _toastNotification.Warning("Route Plan Already Exists!");
                return RedirectToPage("./Details", new { id = CurrentPlan.Id });
            }
            if (Plan.StartDate.DayOfWeek.ToString() != "Monday")
            {
                _toastNotification.Information("Route Plan Start Date Must Be A Monday!");
                return RedirectToPage("./Plans");
            }
            Plan.IsSubmitted = false;
            Plan.FOAstatus = "Pending";
            _context.Plan.Add(Plan);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Route Plan Created!");
            CurrentPlan = await _context.Plan
               .Where(s => s.SalesPersonId == Plan.SalesPersonId && s.StartDate.Date == Plan.StartDate.Date).FirstOrDefaultAsync();

            return RedirectToPage("./Details", new { id = CurrentPlan.Id });
        }
    }
}
