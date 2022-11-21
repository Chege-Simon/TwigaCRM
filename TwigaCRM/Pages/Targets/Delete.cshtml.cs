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

namespace TwigaCRM.Pages.Targets
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
        public Target Target { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id, int salesMovementId)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "remove_target");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            Target = await _context.Target
                .Include(t => t.CropAndAnimal)
                .Include(t => t.Product)
                .Include(t => t.SalesMovement).FirstOrDefaultAsync(m => m.Id == id);
            
            if (Target.SalesMovement.CreatorId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../SalesMovements/SalesMovements");
            }
            if (Target == null)
            {
                return NotFound();
            }
            if (Target != null)
            {
                _context.Target.Remove(Target);
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("Target Removed!");
            }

            return RedirectToPage("../SalesMovements/Details", new {id = salesMovementId });
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Target = await _context.Target.FindAsync(id);

            if (Target != null)
            {
                _context.Target.Remove(Target);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
