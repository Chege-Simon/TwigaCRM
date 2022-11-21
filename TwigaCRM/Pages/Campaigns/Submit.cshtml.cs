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
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.Campaigns
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public DeleteModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        [BindProperty]
        public Campaign Campaign { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "create_campaign");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            Campaign = await _context.Campaign
                .Include(c => c.CampaignBudget)
                .Include(c => c.SalesPerson).FirstOrDefaultAsync(m => m.Id == id);

            if (Campaign.SalesPersonId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../Campaigns/Campaigns");
            }
            Campaign.IsSubmitted = true;
            Campaign.FOAstatus = "Pending";
            Campaign.HRMstatus = "Pending";
            _context.Attach(Campaign).State = EntityState.Modified;
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Campaign Submitted!");

            return RedirectToPage("./Details", new { id });
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Campaign = await _context.Campaign.FindAsync(id);

            if (Campaign != null)
            {
                _context.Campaign.Remove(Campaign);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
