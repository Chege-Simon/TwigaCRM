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
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.DemoProductRequisition
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
        public TwigaCRM.Models.DemoProductRequisition DemoProductRequisition { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "remove_demorequisition");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            DemoProductRequisition = await _context.DemoProductRequisition
                .Include(d => d.Demo)
                .Include(d => d.Product)
                .Include(d => d.Stockist).FirstOrDefaultAsync(m => m.Id == id);

            if (DemoProductRequisition == null)
            {
                _context.DemoProductRequisition.Remove(DemoProductRequisition);
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("Removed Demo Requisition!");
            }
            return RedirectToPage("../Demos/Details", new { id = DemoProductRequisition.DemoId });
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DemoProductRequisition = await _context.DemoProductRequisition.FindAsync(id);

            if (DemoProductRequisition != null)
            {
                _context.DemoProductRequisition.Remove(DemoProductRequisition);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
