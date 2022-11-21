﻿using System;
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

namespace TwigaCRM.Pages.PerformanceReports
{
    [Authorize]
    public class RAMSaleMonthlyPerformanceReportModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public RAMSaleMonthlyPerformanceReportModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }
        public int Week { get; set; }

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
        [BindProperty]
        public RAMDailySaleReport RAMDailySaleReport { get; set; }
        public List<RAMSaleTargetMapping> RAMSaleTargetMappings { get; set; }
        public List<RAMDailySale> RAMDailySales { get; set; }

        public bool IsPermitted { get; private set; }
        public async Task<IActionResult> OnGetAsync(int FinancialYearId, int Month, string RAMId)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "generate_RAM_performance_reports");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
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
            FinancialYear financialYear = await _context.FinancialYear.FirstOrDefaultAsync(f => f.Id == FinancialYearId);
            RAMDailySaleReports = await _context.RAMDailySaleReport
                .Include(d => d.RAM)
                .Include(d => d.RAMDailySales)
                .Where(d => d.RAMId == RAMId && d.SalesDate.Month == Month && d.ApprovalStatus == "Approved").OrderByDescending(s => s.Id).ToListAsync();

            RAMDailySales = await _context.RAMDailySale
                    .Include(d => d.MainDistributor)
                    .Include(d => d.RAMDailySaleReport)
                    .Where(d => d.RAMDailySaleReport.RAMId == RAMId && d.RAMDailySaleReport.SalesDate.Month == Month && (d.RAMDailySaleReport.SalesDate.Date >= financialYear.StartDate.Date && d.RAMDailySaleReport.SalesDate.Date <= financialYear.EndDate.Date) && d.RAMDailySaleReport.ApprovalStatus == "Approved")
                    .OrderByDescending(s => s.Id).ToListAsync();
            RAMSaleTargets = await _context.RAMSaleTarget
                .Include(s => s.FinancialYear)
                .Include(s => s.RAM)
                .Include(s => s.RAMSaleTargetMappings)
                .Where(s => s.ApprovalStatus == "Approved").OrderByDescending(s => s.Id).ToListAsync();

            RAMSaleTargetMappings = await _context.RAMSaleTargetMapping
                    .Include(t => t.MainDistributor)
                    .Include(t => t.RAMSaleTarget)
                    .Where(d => d.RAMSaleTarget.RAMId == RAMId && d.RAMSaleTarget.Month == Month && d.RAMSaleTarget.FinancialYearId == financialYear.Id && d.RAMSaleTarget.ApprovalStatus == "Approved").OrderByDescending(s => s.Id).ToListAsync();
            _toastNotification.Success("Report Ready!");
            return Page();
        }
    }
}
