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
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.RAMPlans
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
        public RAMPlan RAMPlan { get; set; }
        public int RAMPlanId { get; set; }
        public List<RAMRoute> RAMRoutes { get; set; }
        public AppUser AppUser { get; set; }
        public List<Permission> Permissions { get; set; }
        [BindProperty]
        public RAMRoute LastRecordedRoute { get; set; }
        [BindProperty]
        public string ApprovalStatus { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_RAM_route_plan_details");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            RAMPlanId = id;

            RAMPlan = await _context.RAMPlan
                .Include(r => r.RAMRoutes)
                .Include(r => r.RAM)
                .Include(r => r.RAM.Town)
                .Include(r => r.RAM.Town.Region)
                .Include(r => r.RAM.AppRole).FirstOrDefaultAsync(m => m.Id == id);
            RAMRoutes = await _context.RAMRoute.Where(r => r.RAMPlanId == id).Include(r => r.MainDistributor).OrderByDescending(s => s.Id).ToListAsync();
            ApprovalStatus = RAMPlan.ApprovalStatus;
            //LastRecordedRoute = await _context.RAMRoute.Where(r => r.RAMPlan.Id == RAMPlan.Id).OrderByDescending(r => r.RouteDate).FirstOrDefaultAsync();
            //if (LastRecordedRoute == null)
            //{
            //    LastRecordedRoute = new RAMRoute();
            //    LastRecordedRoute.RouteDate = RAMPlan.StartDate.AddDays(-1);
            //}
            if (RAMPlan == null)
            {
                return NotFound();
            }
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).ToListAsync();
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
            Console.WriteLine("#######This is the RAM Plan Status:   "+RAMPlan.ApprovalStatus);
            return Page();
        }
        [BindProperty]
        public RAMRoute RAMRoute { get; set; }

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

            RAMPlan = await _context.RAMPlan
            .Include(p => p.RAM)
                .Include(p => p.RAMRoutes).FirstOrDefaultAsync(m => m.Id == RAMRoute.RAMPlanId);

            if (RAMPlan.RAMId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../RAMSaleTargets/RAMSaleTargets");
            }
            RAMRoute.Day = RAMRoute.RouteDate.DayOfWeek.ToString();
            //RAMRoute.Day = LastRecordedRoute.RouteDate.AddDays(1).DayOfWeek.ToString();
            //RAMRoute.RouteDate = LastRecordedRoute.RouteDate.AddDays(1);
            //var UserId = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            //var CurrentRoute = await _context.Route.Include(r => r.Plan).Where(d => d.PlanId == RAMRoute.RAMPlanId && d.RouteDate.Date == RAMRoute.RouteDate.Date).FirstOrDefaultAsync();
            //if (CurrentRoute != null)
            //{
            //    _toastNotification.Warning("Route Already Exists!");
            //    return RedirectToPage("./Details", new { id = CurrentRoute.Id });
            //}
            _context.RAMRoute.Add(RAMRoute);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Route Created!");

            return RedirectToPage("./Details", new { id = RAMRoute.RAMPlanId });
        }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD;
        [BindProperty]
        public RAMRoute LocationRoute { get; set; }
        [BindProperty]
        public string ActualLat { get; set; }
        [BindProperty]
        public string ActualLong { get; set; }
        public async Task<IActionResult> OnPostAddLocationAsync(int id)
        {
            //if (!ModelState.IsValid)
            //{
            //    _toastNotification.Error("Invalid Inputs!");
            //    return RedirectToPage("./Plans");
            //}

            IsPermitted = _checkPermissions.CheckPermission(User, "create_RAM_route_plan");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            var CurrentRoute = await _context.RAMRoute.Include(r => r.RAMPlan).FirstOrDefaultAsync(r => r.Id == id);
            if (ActualLat == null || ActualLong == null)
            {
                _toastNotification.Error("Missed Coordinates!");
                    return RedirectToPage("../RAMPlans");
            }

            if (RAMPlan.RAMId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../RAMPlans/RAMPlans");
            }
            CurrentRoute.ActualLat = ActualLat;
            CurrentRoute.ActualLong = ActualLong;
            _context.Attach(CurrentRoute).State = EntityState.Modified;
            //_context.Route.Add(CurrentRoute);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Route Location Added!");

            return RedirectToPage("./Details", new { id = CurrentRoute.RAMPlanId });
        }


        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "approve_RAM_route_plan");
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
            RAMPlan = await _context.RAMPlan.FirstOrDefaultAsync(d => d.Id == id);

            List<RAMRoute> reqRoutes = await _context.RAMRoute
                .Include(r => r.RAMPlan)
                .Include(r => r.Zone)
                .Where(r => r.RAMPlanId == RAMPlan.Id).ToListAsync();
            foreach (var item in reqRoutes)
            {
                if (!item.IsApproved)
                {
                    _toastNotification.Information("Route: " + item.Zone.Name + " On " + item.Day + " NOT APPROVED!");
                    return RedirectToPage("./Details", new { id });
                }
            }
            RAMPlan.IsSubmitted = ApprovalStatus != "Rejected" ? true : false;
            RAMPlan.ApprovalStatus = ApprovalStatus;
            _context.Attach(RAMPlan).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Route Approval Changed!");

            return RedirectToPage("./Details", new { id });
        }
        [BindProperty]
        public string Remarks { get; set; }
        public async Task<IActionResult> OnPostAddRemarksAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_RAM_route_plan_remarks");
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

            RAMPlan = await _context.RAMPlan.FirstOrDefaultAsync(d => d.Id == id);
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);

            RAMPlan.Remarks = RAMPlan.Remarks + System.Environment.NewLine + AppUser.FirstName + " " + AppUser.LastName + " ( " + AppUser.AppRole.Title + ") : " + Remarks;
            _context.Attach(RAMPlan).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Remarks Added!");

            return RedirectToPage("./Details", new { id });
        }
    }
}
