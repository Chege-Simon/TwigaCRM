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
using Route = TwigaCRM.Models.Route;

namespace TwigaCRM.Pages.RAMRoutes
{
    [Authorize]
    public class SubmitModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public SubmitModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        [BindProperty]
        public RAMRoute RAMRoute { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id, int planId)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "approve_RAM_route_plan");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            RAMRoute = await _context.RAMRoute
                .Include(c => c.Zone)
                .Include(c => c.RAMPlan).FirstOrDefaultAsync(m => m.Id == id);

            RAMRoute.IsApproved = true;
            _context.Attach(RAMRoute).State = EntityState.Modified;
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("RAM Route Approved!");

            return RedirectToPage("../RAMPlans/Details", new { id = planId });
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RAMRoute = await _context.RAMRoute.FindAsync(id);

            if (RAMRoute != null)
            {
                _context.RAMRoute.Remove(RAMRoute);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
