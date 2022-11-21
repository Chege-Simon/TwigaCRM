using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.RAMPlans
{
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
        public RAMPlan RAMPlan { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "create_RAM_route_plan");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            RAMPlan = await _context.RAMPlan
                .Include(c => c.RAM).FirstOrDefaultAsync(m => m.Id == id);


            if (RAMPlan.RAMId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../RAMPlans/RAMPlans");
            }
            RAMPlan.IsSubmitted = true;
            RAMPlan.ApprovalStatus = "Pending";
            _context.Attach(RAMPlan).State = EntityState.Modified;
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Route Plan Submitted!");

            return RedirectToPage("./Details", new { id });
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RAMPlan = await _context.RAMPlan.FindAsync(id);

            if (RAMPlan != null)
            {
                _context.RAMPlan.Remove(RAMPlan);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
