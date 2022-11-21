using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
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
using Route = TwigaCRM.Models.Route;

namespace TwigaCRM.Pages.RoutePlanReports
{
    [Authorize]
    public class TMSSPReportModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public TMSSPReportModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        public List<Route> Routes { get; set; }


        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required]
            [Display(Name = "Start Date")]
            public DateTime StartDate { get; set; }

            [Required]
            [Display(Name = "End Date")]
            public DateTime EndDate { get; set; }


        }
        public bool IsPermitted { get; private set; }
        public DateTime StartedDate { get; set; }

        public DateTime EndedDate { get; set; }
        public async Task<IActionResult> OnGetAsync(DateTime startdate, DateTime enddate)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "generate_demo_reports");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);

            StartedDate = startdate;
            EndedDate = enddate;
            Routes = await _context.Route.Include(d => d.Plan)
                    .Include(d => d.Zone)
                    .Include(d => d.Zone.Town)
                    .Include(d => d.Zone.Town.Region)
                    .Include(d => d.Plan)
                    .Include(d => d.Plan.SalesPerson)
                    .Where(d => d.RouteDate.Date >= StartedDate.Date && d.RouteDate.Date <= EndedDate.Date && d.Plan.FOAstatus == "Approved").OrderByDescending(s => s.Id).ToListAsync();

            _toastNotification.Success("Report Ready!");
            return Page();
        }
    }
}
