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
using Microsoft.Extensions.Hosting;
using NToastNotify;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.DemoProgressReports
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private IHostEnvironment _environment;
        private readonly CheckPermissionsService _checkPermissions;
        private readonly INotyfService _toastNotification;

        public IndexModel(TwigaCRM.Data.ApplicationDbContext context, IHostEnvironment environment, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _environment = environment;
            _checkPermissions = checkPermissions;
            _toastNotification = toastNotification;
        }

        public IList<DemoProgressReport> DemoProgressReports { get;set; }
        public DemoDetail DemoDetail { get; set; }
        [BindProperty]
        public DemoProgressReport DemoProgressReport { get; set; }
        [BindProperty]
        public int DemoId { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id, int demoId)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "demo_reports");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            DemoId = demoId;
            DemoProgressReport = new DemoProgressReport();
            DemoDetail = await _context.DemoDetail.FirstOrDefaultAsync(e => e.Id == id);
            DemoProgressReport.DemoDetailId = DemoDetail.Id;
            DemoProgressReports = await _context.DemoProgressReport
                .Include(e => e.DemoDetail)
                .Include(e => e.DemoDetail.Demo)
                .Where(e => e.DemoDetailId == id).ToListAsync();
            return Page();
        }


        [BindProperty]
        public IFormFile Upload { get; set; }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            Demo Demo = await _context.Demo
                .Include(c => c.SalesPerson).FirstOrDefaultAsync(m => m.Id == DemoProgressReport.DemoDetail.DemoId);
            if (Demo.SalesPersonId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../Demos/Demos");
            }
            var fileName = DateTime.Now.Ticks + Upload.FileName;
            System.IO.Directory.CreateDirectory("uploads");
            var file = Path.Combine(_environment.ContentRootPath, "uploads", fileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }
            DemoProgressReport.DocumentUrl = fileName;
            _context.DemoProgressReport.Add(DemoProgressReport);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Report Added!");
            DemoDetail = await _context.DemoDetail.FirstOrDefaultAsync(e => e.Id == DemoProgressReport.DemoDetailId);

            return RedirectToPage("./DemoProgressReports", new { id = DemoProgressReport.DemoDetailId, campaignId = DemoProgressReport.DemoDetail.DemoId });
        }
        public FileResult OnGetDownloadFile(string fileName)
        {
            //Build the File Path.
            string path = Path.Combine(_environment.ContentRootPath, "uploads/") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            _toastNotification.Success("Report Downloaded!");
            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }
    }
}
