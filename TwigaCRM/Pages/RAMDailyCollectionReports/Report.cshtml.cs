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

namespace TwigaCRM.Pages.RAMDailyCollectionReports
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

        public List<RAMDailyCollection> RAMDailyCollections { get; set; }
        public List<Customer> Customers { get; set; }
        public List<RAMCollectionTargetMapping> RAMCollectionTargetMappings { get; set; }

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

        public DateTime StartedDate { get; set; }

        public DateTime EndedDate { get; set; }
        public bool IsPermitted { get; private set; }
        public async Task<IActionResult> OnGetAsync(DateTime startdate, DateTime enddate)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "generate_RAM_performance_reports");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            StartedDate = startdate;
            EndedDate = enddate;

            Customers = await _context.Customer.ToListAsync();
            RAMCollectionTargetMappings = await _context.RAMCollectionTargetMapping.Include(t => t.MainDistributor).Include(t => t.RAMCollectionTarget).Include(t => t.RAMCollectionTarget.RAM).Where(d => (d.RAMCollectionTarget.CreateAt.Date >= StartedDate.Date || d.RAMCollectionTarget.UpdateAt.Date >= StartedDate.Date) && (d.RAMCollectionTarget.CreateAt.Date >= EndedDate.Date || d.RAMCollectionTarget.UpdateAt.Date >= EndedDate.Date) && d.RAMCollectionTarget.ApprovalStatus == "Approved").ToListAsync();

            RAMDailyCollections = await _context.RAMDailyCollection
                    .Include(d => d.RAMDailyCollectionReport)
                    .Include(d => d.MainDistributor)
                    .Include(d => d.MainDistributor.Town)
                    .Include(d => d.MainDistributor.Town.Region)
                    .Where(d => d.RAMDailyCollectionReport.SalesDate.Date >= StartedDate.Date && d.RAMDailyCollectionReport.SalesDate.Date <= EndedDate.Date && d.RAMDailyCollectionReport.ApprovalStatus == "Approved").ToListAsync();
           
            _toastNotification.Success("Report Ready!");
            return Page();
        }
    }
}
