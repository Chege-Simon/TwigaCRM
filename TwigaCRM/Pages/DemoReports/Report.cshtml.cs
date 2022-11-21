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

namespace TwigaCRM.Pages.DemoReports
{
    [Authorize]
    public class ReportModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public ReportModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }
        public int Week { get; set; }

        public List<Demo> Demos { get; set; }


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
            Demos = await _context.Demo
                    .Include(d => d.SalesPerson)
                    .Where(d => d.StartDate.Date >= StartedDate.Date && d.EndDate.Date <= EndedDate.Date && d.IsSubmitted).OrderByDescending(s => s.Id).ToListAsync();

            _toastNotification.Success("Report Ready!");
            return Page();
        }
    }
}
