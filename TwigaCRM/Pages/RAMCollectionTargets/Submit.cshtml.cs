using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.RAMCollectionTargets
{
    [Authorize]
    public class SubmitModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public SubmitModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        [BindProperty]
        public RAMCollectionTarget RAMCollectionTarget { get; set; }
        public bool IsPermitted { get; private set; }
        public AppUser AppUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "create_RAM_collection_target");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);

            RAMCollectionTarget = await _context.RAMCollectionTarget
                .Include(s => s.FinancialYear)
                .Include(s => s.RAM).FirstOrDefaultAsync(m => m.Id == id);

            if (RAMCollectionTarget == null)
            {
                return NotFound();
            }

            if (RAMCollectionTarget.RAMId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../RAMCollectionTargets/RAMCollectionTargets");
            }

            RAMCollectionTarget = await _context.RAMCollectionTarget
                .Include(d => d.RAM).FirstOrDefaultAsync(m => m.Id == id);
            RAMCollectionTarget.IsSubmitted = true;
            RAMCollectionTarget.ApprovalStatus = "Pending";
            _context.Attach(RAMCollectionTarget).State = EntityState.Modified;
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Collection Target Submitted!");

            return RedirectToPage("../RAMCollectionTargets/Details", new { id });
        }

    }
}
