using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.RAMCollectionTargetMappings
{
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
        public RAMCollectionTargetMapping RAMCollectionTargetMapping { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id, int RAMsaleTargetId)
        {

            IsPermitted = _checkPermissions.CheckPermission(User, "remove_RAM_collection_target");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            RAMCollectionTargetMapping = await _context.RAMCollectionTargetMapping
                .Include(t => t.RAMCollectionTarget).FirstOrDefaultAsync(m => m.Id == id);

            if (RAMCollectionTargetMapping.RAMCollectionTarget.RAMId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../RAMCollectionTargets/RAMCollectionTargets");
            }
            if (RAMCollectionTargetMapping == null)
            {
                return NotFound();
            }
            if (RAMCollectionTargetMapping != null)
            {
                _context.RAMCollectionTargetMapping.Remove(RAMCollectionTargetMapping);
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("Target Removed!");
            }

            return RedirectToPage("../RAMCollectionTargets/Details", new { id = RAMsaleTargetId });
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RAMCollectionTargetMapping = await _context.RAMCollectionTargetMapping.FindAsync(id);

            if (RAMCollectionTargetMapping != null)
            {
                _context.RAMCollectionTargetMapping.Remove(RAMCollectionTargetMapping);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
