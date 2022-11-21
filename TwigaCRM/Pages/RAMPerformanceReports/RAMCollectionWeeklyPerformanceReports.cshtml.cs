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
    public class RAMCollectionWeeklyPerformanceReportModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public RAMCollectionWeeklyPerformanceReportModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        public List<RAMDailyCollectionReport> RAMDailyCollectionReports { get; set; }
        public IList<RAMCollectionTarget> RAMCollectionTargets { get; set; }


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
        public RAMDailyCollectionReport RAMDailyCollectionReport { get; set; }
        public List<RAMCollectionTargetMapping> RAMCollectionTargetMappings { get; set; }
        public List<RAMDailyCollection> RAMDailyCollections { get; set; }
        public IList<AppRole> AppRoles { get; set; } = default!;
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
            RAMDailyCollectionReports = await _context.RAMDailyCollectionReport
                .Include(d => d.RAM)
                .Include(d => d.RAMDailyCollections)
                .Where(d => d.RAMId == RAMId && d.SalesDate.Month == Month && d.ApprovalStatus == "Approved").ToListAsync();

            RAMDailyCollections = await _context.RAMDailyCollection
                    .Include(t => t.MainDistributor)
                    .Include(d => d.RAMDailyCollectionReport)
                    .Where(d => d.RAMDailyCollectionReport.RAMId == RAMId && d.RAMDailyCollectionReport.SalesDate.Month == Month && (d.RAMDailyCollectionReport.SalesDate.Date >= financialYear.StartDate.Date && d.RAMDailyCollectionReport.SalesDate.Date <= financialYear.EndDate.Date) && d.RAMDailyCollectionReport.ApprovalStatus == "Approved").ToListAsync();
            RAMCollectionTargets = await _context.RAMCollectionTarget
                .Include(s => s.FinancialYear)
                .Include(s => s.RAM)
                .Include(s => s.RAMCollectionTargetMappings)
                .Where(s => s.ApprovalStatus == "Approved").ToListAsync();

            RAMCollectionTargetMappings = await _context.RAMCollectionTargetMapping
                    .Include(t => t.MainDistributor)
                    .Include(t => t.RAMCollectionTarget)
                    .Where(d => d.RAMCollectionTarget.RAMId == RAMId && d.RAMCollectionTarget.Month == Month && d.RAMCollectionTarget.FinancialYearId == financialYear.Id && d.RAMCollectionTarget.ApprovalStatus == "Approved").ToListAsync();
           
            _toastNotification.Success("Report Ready!");
            return Page();
        }
    }
}
