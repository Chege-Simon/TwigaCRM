﻿using System;
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
    public class PerformanceReportsModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public PerformanceReportsModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        public List<DailyMovementReport> DailyMovementReports { get; set; }
        public IList<SalesMovement> SalesMovements { get; set; }


        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required]
            [Display(Name = "SalesPerson")]
            public string SalesPersonId { get; set; }

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

            IsPermitted = _checkPermissions.CheckPermission(User, "generate_performance_reports");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);

            DailyMovementReports = await _context.DailyMovementReport
                .Include(d => d.SalesPerson).ToListAsync();

            SalesMovements = await _context.SalesMovement
                .Include(s => s.FinancialYear)
                .Include(s => s.SalesPerson).ToListAsync();
            ViewData["FinancialYears"] = _context.FinancialYear.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.StartDate.Year + " - " + a.EndDate.Year
                                            }).ToList();
            ViewData["SalesPersons"] = _userManager.Users.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.FirstName + " " + a.LastName
                                            }).ToList();

            return Page();
        }
        public List<Target> Targets { get; set; }
        public List<DailyMovement> DailyMovements { get; set; }
        public List<ReportModel> Reports { get; set; }
        public class ReportModel
        {
            [Required]
            [Display(Name = "Product")]
            public int ProductId { get; set; }
            public Product Product { get; set; }

            [Required]
            [Display(Name = "ActualMovement")]
            public decimal ActualMovement { get; set; }

            [Required]
            [Display(Name = "TargetMovement")]
            public decimal TargetMovement { get; set; }

        }
        public List<MonthlyMovement> Movements { get; set; }
        public class MonthlyMovement
        {
            public int ProductId { get; set; }
            public Product Product { get; set; }

            public decimal MonthMovement { get; set; }
        }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "generate_performance_reports");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            FinancialYear theFinancialYear = await _context.FinancialYear.FirstOrDefaultAsync(f => f.Id == Input.FinancialYearId);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./PerformanceReports");
            }


            return RedirectToPage("./MonthlyPerformanceReports", new { FinancialYearId = Input.FinancialYearId, Month = Input.Month, SalesPersonId = Input.SalesPersonId });
        }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostWeeklyPerformanceAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "generate_performance_reports");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            FinancialYear theFinancialYear = await _context.FinancialYear.FirstOrDefaultAsync(f => f.Id == Input.FinancialYearId);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./PerformanceReports");
            }


            return RedirectToPage("./WeeklyPerformanceReports", new { FinancialYearId = Input.FinancialYearId, Month = Input.Month, SalesPersonId = Input.SalesPersonId });
        }
    }
}
