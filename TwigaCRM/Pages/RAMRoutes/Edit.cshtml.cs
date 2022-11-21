using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
using Route = TwigaCRM.Models.Route;

namespace TwigaCRM.Pages.RAMRoutes
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;

        private readonly CheckPermissionsService _checkPermissions;
        public EditModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        [BindProperty]
        public RAMRoute RAMRoute { get; set; }
        public AppUser AppUser { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_RAM_route");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            RAMRoute = await _context.RAMRoute
                .Include(r => r.RAMPlan)
                .Include(r => r.Zone).FirstOrDefaultAsync(m => m.Id == id);

            if (RAMRoute == null)
            {
                return NotFound();
            }
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.Town.Zones)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            ViewData["Zones"] = AppUser.Town.Zones.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name + " @ " + a.Town.Name + " - " + a.Town.Region.Name
                                            }).ToList();
            ViewData["MainDistributors"] = _context.Customer.Include(c => c.Town).Include(c => c.Town.Region).Where(c => c.CustomerType == "Main Distributor" && c.Town.RegionId == AppUser.Town.RegionId).Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.ContactPersonName + " @ " + a.SiteName
                                            }).ToList();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_RAM_route");
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
            RAMPlan RAMPlan = await _context.RAMPlan.Include(r => r.RAM).FirstOrDefaultAsync(r => r.Id == RAMRoute.RAMPlanId);
            if (RAMPlan.RAMId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../RAMPlans/RAMPlans");
            }
            _context.Attach(RAMRoute).State = EntityState.Modified;

            try
            {
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("RAMRoute Edited!");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RAMRouteExists(RAMRoute.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../RAMPlans/Details", new { id = RAMRoute.RAMPlanId });
        }

        private bool RAMRouteExists(int id)
        {
            return _context.RAMRoute.Any(e => e.Id == id);
        }
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostApproveAsync(int id, int planId)
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}
            RAMRoute = await _context.RAMRoute.FirstOrDefaultAsync(r => r.Id == id);
            RAMRoute.IsApproved = true;
            _context.Attach(RAMRoute).State = EntityState.Modified;

            try
            {
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("RAM Route Approved!");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RAMRouteExists(RAMRoute.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Plans/Details", new {id = planId});
        }
    }
}
