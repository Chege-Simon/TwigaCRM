using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.DemoProgressReports
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly CheckPermissionsService _checkPermissions;
        private readonly INotyfService _toastNotification;

        public DeleteModel(TwigaCRM.Data.ApplicationDbContext context, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _checkPermissions = checkPermissions;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public DemoProgressReport DemoProgressReport { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "remove_demo_progress_report");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            DemoProgressReport = await _context.DemoProgressReport
                .Include(d => d.DemoDetail)
                .Include(d => d.DemoDetail.Demo).FirstOrDefaultAsync(m => m.Id == id);

            Demo Demo = await _context.Demo
                .Include(c => c.SalesPerson).FirstOrDefaultAsync(m => m.Id == DemoProgressReport.DemoDetail.DemoId);
            if (Demo.SalesPersonId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../Demos/Demos");
            }

            if (DemoProgressReport != null)
            {
                _context.DemoProgressReport.Remove(DemoProgressReport);
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("Report Removed!");
            }
            return RedirectToPage("./DemoProgressReports", new { id = DemoProgressReport.DemoDetailId, demoId = DemoProgressReport.DemoDetail.DemoId });
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DemoProgressReport = await _context.DemoProgressReport.FindAsync(id);

            if (DemoProgressReport != null)
            {
                _context.DemoProgressReport.Remove(DemoProgressReport);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
