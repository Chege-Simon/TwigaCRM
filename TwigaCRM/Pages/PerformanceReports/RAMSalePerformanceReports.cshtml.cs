using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Xml.Linq;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using NuGet.Packaging;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.PerformanceReports
{
    [Authorize]
    public class RAMSalePerformanceReportsModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public RAMSalePerformanceReportsModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        public List<RAMDailySaleReport> RAMDailySaleReports { get; set; }
        public IList<RAMSaleTarget> RAMSaleTargets { get; set; }


        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required]
            [Display(Name = "RAM")]
            public string RAMId { get; set; }

            [Required]
            [Display(Name = "Month")]
            public int Month { get; set; }

            [Required]
            [Display(Name = "FinancialYear")]
            public int FinancialYearId { get; set; }

        }
        public bool IsPermitted { get; private set; }
        public async Task<IActionResult> OnGetAsync()
        {

            IsPermitted = _checkPermissions.CheckPermission(User, "generate_RAM_performance_reports");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);

            RAMDailySaleReports = await _context.RAMDailySaleReport
                .Include(d => d.RAM).ToListAsync();

            RAMSaleTargets = await _context.RAMSaleTarget
                .Include(s => s.FinancialYear)
                .Include(s => s.RAM).ToListAsync();
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

            return Page();
        }
        public List<Target> Targets { get; set; }
        public List<RAMDailySale> RAMDailySales { get; set; }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "generate_RAM_performance_reports");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            FinancialYear theFinancialYear = await _context.FinancialYear.FirstOrDefaultAsync(f => f.Id == Input.FinancialYearId);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./RAMSalePerformanceReports");
            }


            return RedirectToPage("./RAMSaleMonthlyPerformanceReports", new { FinancialYearId = Input.FinancialYearId, Month = Input.Month, RAMId = Input.RAMId });
        }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostWeeklyPerformanceAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "generate_RAM_performance_reports");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            FinancialYear theFinancialYear = await _context.FinancialYear.FirstOrDefaultAsync(f => f.Id == Input.FinancialYearId);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./RAMSalePerformanceReports");
            }


            return RedirectToPage("./RAMSaleWeeklyPerformanceReports", new { FinancialYearId = Input.FinancialYearId, Month = Input.Month, RAMId = Input.RAMId });
        }
    }
}
