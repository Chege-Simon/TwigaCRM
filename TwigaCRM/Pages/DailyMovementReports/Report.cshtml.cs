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

namespace TwigaCRM.Pages.DailyMovementReports
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

        public List<DailyMovement> DailyMovements { get; set; }
        public IList<SalesMovement> SalesMovements { get; set; }
        public List<Target> Targets { get; set; }
        public List<Customer> Customers { get; set; }

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
            IsPermitted = _checkPermissions.CheckPermission(User, "generate_performance_reports");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            StartedDate = startdate;
            EndedDate = enddate;

            Customers = await _context.Customer.Include(c => c.Town).Include(c => c.Town.Region).OrderByDescending(s => s.Id).ToListAsync();
            Targets = await _context.Target.Include(t => t.Product).Include(t => t.SalesMovement).Include(t => t.SalesMovement.SalesPerson).Where(d => (d.SalesMovement.CreateAt.Date >= StartedDate.Date || d.SalesMovement.UpdateAt.Date >= StartedDate.Date) && (d.SalesMovement.CreateAt.Date >= EndedDate.Date || d.SalesMovement.UpdateAt.Date >= EndedDate.Date) && d.SalesMovement.TLstatus == "Approved").OrderByDescending(s => s.Id).ToListAsync();
            DailyMovements = await _context.DailyMovement
                    .Include(d => d.Product)
                    .Include(d => d.DailyMovementReport)
                    .Include(d => d.MainDistributor)
                    .Include(d => d.DailyMovementReport.SalesPerson)
                    .Include(d => d.MainDistributor.Town)
                    .Include(d => d.MainDistributor.Town.Region)
                    .Where(d => d.DailyMovementReport.SalesDate.Date >= StartedDate.Date && d.DailyMovementReport.SalesDate.Date <= EndedDate.Date && d.DailyMovementReport.TLstatus == "Approved").OrderByDescending(s => s.Id).ToListAsync();
           
            _toastNotification.Success("Report Ready!");
            return Page();
        }
    }
}
