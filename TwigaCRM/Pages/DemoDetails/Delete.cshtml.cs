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

namespace TwigaCRM.Pages.DemoDetails
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly CheckPermissionsService _checkPermissions;
        private readonly INotyfService _toastNotification;

        public DeleteModel(TwigaCRM.Data.ApplicationDbContext context, CheckPermissionsService checkPermissions, INotyfService toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        [BindProperty]
        public DemoDetail DemoDetail { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "remove_demodetail");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            DemoDetail = await _context.DemoDetail
                .Include(d => d.CompetingProduct)
                .Include(d => d.Demo)
                .Include(d => d.Product).FirstOrDefaultAsync(m => m.Id == id);

            Demo Demo = await _context.Demo
                .Include(c => c.SalesPerson).FirstOrDefaultAsync(m => m.Id == DemoDetail.DemoId);
            if (Demo.SalesPersonId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../Demos/Demos");
            }
            if (DemoDetail != null)
            {
                _context.DemoDetail.Remove(DemoDetail);
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("Removed Details!");
            }

            return RedirectToPage("../Demos/Details", new {id = DemoDetail.DemoId});
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DemoDetail = await _context.DemoDetail.FindAsync(id);

            if (DemoDetail != null)
            {
                _context.DemoDetail.Remove(DemoDetail);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
