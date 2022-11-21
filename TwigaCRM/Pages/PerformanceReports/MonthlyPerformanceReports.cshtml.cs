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

namespace TwigaCRM.Pages.PerformanceReports
{
    [Authorize]
    public class MonthlyPerformanceReportModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public MonthlyPerformanceReportModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }
        public int Week { get; set; }

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
        [BindProperty]
        public DailyMovementReport DailyMovementReport { get; set; }
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

        public bool IsPermitted { get; private set; }
        public async Task<IActionResult> OnGetAsync(int FinancialYearId, int Month, string SalesPersonId)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "generate_performance_reports");
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
            ViewData["SalesPersons"] = _userManager.Users.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.FirstName + " " + a.LastName
                                            }).ToList();
            FinancialYear financialYear = await _context.FinancialYear.FirstOrDefaultAsync(f => f.Id == FinancialYearId);
            DailyMovementReports = await _context.DailyMovementReport
                .Include(d => d.SalesPerson)
                .Include(d => d.DailyMovements)
                .Where(d => d.SalesPersonId == SalesPersonId && d.SalesDate.Month == Month && d.TLstatus == "Approved").OrderByDescending(s => s.Id).ToListAsync();

            DailyMovements = await _context.DailyMovement
                    .Include(d => d.Product)
                    .Include(d => d.DailyMovementReport)
                    .Include(d => d.DailyMovementReport.SalesPerson)
                    .Where(d => d.DailyMovementReport.SalesPersonId == SalesPersonId && d.DailyMovementReport.SalesDate.Month == Month && (d.DailyMovementReport.SalesDate.Date >= financialYear.StartDate.Date && d.DailyMovementReport.SalesDate.Date <= financialYear.EndDate.Date) && d.DailyMovementReport.TLstatus == "Approved").OrderByDescending(s => s.Id).ToListAsync();
            SalesMovements = await _context.SalesMovement
                .Include(s => s.FinancialYear)
                .Include(s => s.SalesPerson)
                .Include(s => s.Targets)
                .Where(s => s.TLstatus == "Approved").OrderByDescending(s => s.Id).ToListAsync();

            Targets = await _context.Target
                    .Include(t => t.Product)
                    .Include(t => t.SalesMovement)
                    .Include(t => t.SalesMovement.SalesPerson)
                    .Where(d => d.SalesMovement.SalesPersonId == SalesPersonId && d.SalesMovement.Month == Month && d.SalesMovement.FinancialYearId == financialYear.Id && d.SalesMovement.TLstatus == "Approved").OrderByDescending(s => s.Id).ToListAsync();
            _toastNotification.Success("Report Ready!");
            return Page();
        }
    }
}
