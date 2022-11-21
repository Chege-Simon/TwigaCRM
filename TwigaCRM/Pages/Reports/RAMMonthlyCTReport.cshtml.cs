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

namespace TwigaCRM.Pages.Reports
{
    [Authorize]
    public class RAMMonthlyCTReportModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public RAMMonthlyCTReportModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }
        public int Week { get; set; }

        public List<RAMCollectionTargetMapping> RAMCollectionTargetMappings { get; set; }
        public IList<RAMCollectionTarget> RAMCollectionTargets { get; set; }
        public List<Customer> Customers { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required]
            [Display(Name = "Month")]
            public int Month { get; set; }

            [Required]
            [Display(Name = "FinancialYear")]
            public int FinancialYearId { get; set; }
        }

        public int Month { get; set; }

        public DateTime EndedDate { get; set; }
        public bool IsPermitted { get; private set; }
        public async Task<IActionResult> OnGetAsync(int month, int financialYearId)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "generate_sales_target_report");
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
            Month = month;
            FinancialYear financialYear = await _context.FinancialYear.FirstOrDefaultAsync(f => f.Id == financialYearId);

            Customers = await _context.Customer.ToListAsync();

            RAMCollectionTargetMappings = await _context.RAMCollectionTargetMapping
                   .Include(t => t.RAMCollectionTarget)
                   .Where(d => d.RAMCollectionTarget.Month == Month && d.RAMCollectionTarget.FinancialYearId == financialYear.Id && d.RAMCollectionTarget.ApprovalStatus == "Approved")
                   .OrderByDescending(s => s.CreateAt).ToListAsync();

            _toastNotification.Success("Report Ready!");
            return Page();
        }
    }
}
