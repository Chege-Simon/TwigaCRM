using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;
using Route = TwigaCRM.Models.Route;

namespace TwigaCRM.Pages.Plans
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

        public Plan Plan { get; set; }
        public int PlanId { get; set; }
        public AppUser AppUser { get; set; }
        public List<Permission> Permissions { get; set; }
        [BindProperty]
        public Route LastRecordedRoute { get; set; }
        [BindProperty]
        public string FOAstatus { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_route_plan_details");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            PlanId = id;
            Plan = await _context.Plan
                .Include(p => p.SalesPerson)
                .Include(p => p.SalesPerson.AppRole)
                .Include(p => p.SalesPerson.Town)
                .Include(p => p.SalesPerson.Town.Region)
                .Include(p => p.SalesPerson.Town.Zones)
                .Include(p => p.Routes).FirstOrDefaultAsync(m => m.Id == id);
            FOAstatus = Plan.FOAstatus;
            //LastRecordedRoute = await _context.Route.Where(r => r.Plan.Id == Plan.Id).OrderByDescending(r => r.RouteDate).FirstOrDefaultAsync();
            //if (LastRecordedRoute == null)
            //{
            //    LastRecordedRoute = new Route();
            //    LastRecordedRoute.RouteDate = Plan.StartDate.AddDays(-1);
            //}
            if (Plan == null)
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
            return Page();
        }

        [BindProperty]
        public Route Route { get; set; }

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
                return RedirectToPage("./Plans");
            }

            Plan = await _context.Plan
            .Include(p => p.SalesPerson)
                .Include(p => p.Routes).FirstOrDefaultAsync(m => m.Id == Route.PlanId);
            if (Plan.SalesPersonId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../Plans/Plans");
            }
            Route.Day = Route.RouteDate.DayOfWeek.ToString();
            //Route.Day = LastRecordedRoute.RouteDate.AddDays(1).DayOfWeek.ToString();
            //Route.RouteDate = LastRecordedRoute.RouteDate.AddDays(1);
            //var UserId = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            //var CurrentRoute = await _context.Route.Include(r => r.Plan).Where(d => d.PlanId == Route.PlanId && d.RouteDate.Date == Route.RouteDate.Date).FirstOrDefaultAsync();
            //if (CurrentRoute != null)
            //{
            //    _toastNotification.Warning("Route Already Exists!");
            //    return RedirectToPage("./Details", new { id = CurrentRoute.Id });
            //}
            _context.Route.Add(Route);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Route Created!");

            return RedirectToPage("./Details", new {id = Route.PlanId});
        }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD;
        [BindProperty]
        public Route LocationRoute { get; set; }
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

            IsPermitted = _checkPermissions.CheckPermission(User, "create_route_plan");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            var CurrentRoute = await _context.Route.Include(r => r.Plan).FirstOrDefaultAsync(r => r.Id == id);
            CurrentRoute.ActualLat = ActualLat;
            CurrentRoute.ActualLong = ActualLong;

            if (CurrentRoute.Plan.SalesPersonId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../Plans/Plans");
            }
            _context.Attach(CurrentRoute).State = EntityState.Modified;
            //_context.Route.Add(CurrentRoute);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Route Location Added!");

            return RedirectToPage("./Details", new { id = CurrentRoute.PlanId });
        }


        public async Task<IActionResult> OnPostFOAApproveAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "foa_approve_route_plan");
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
            Plan = await _context.Plan.FirstOrDefaultAsync(d => d.Id == id);

            List<Route> reqRoutes = await _context.Route
                .Include(r => r.Plan)
                .Include(r => r.Zone)
                .Where(r => r.PlanId == Plan.Id).ToListAsync();
            foreach (var item in reqRoutes)
            {
                if (!item.IsFOAApproved)
                {
                    _toastNotification.Information("Route: " + item.Zone.Name+" On "+ item.Day + " NOT APPROVED!");
                    return RedirectToPage("./Details", new { id });
                }
            }
            Plan.IsSubmitted = FOAstatus != "Rejected" ? true : false;
            Plan.FOAstatus = FOAstatus;
            _context.Attach(Plan).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Route Approval Changed!");

            return RedirectToPage("./Details", new { id });
        }
        [BindProperty]
        public string Remarks { get; set; }
        public async Task<IActionResult> OnPostAddRemarksAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_route_plan_remarks");
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

            Plan = await _context.Plan.FirstOrDefaultAsync(d => d.Id == id);
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);

            Plan.Remarks = Plan.Remarks + System.Environment.NewLine + AppUser.FirstName + " " + AppUser.LastName + " ( " + AppUser.AppRole.Title + ") : " + Remarks;
            _context.Attach(Plan).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Remarks Added!");

            return RedirectToPage("./Details", new { id });
        }
    }
}
