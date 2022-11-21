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

namespace TwigaCRM.Pages.RequestedCampaignItems
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
        public RequestedCampaignItem RequestedCampaignItem { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id, int campaignId)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "remove_requested_campaign_item");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            RequestedCampaignItem = await _context.RequestedCampaignItem
                .Include(r => r.Campaign)
                .Include(r => r.CampaignItem).FirstOrDefaultAsync(m => m.Id == id);

            Campaign Campaign = await _context.Campaign
                .Include(c => c.CampaignBudget)
                .Include(c => c.SalesPerson).FirstOrDefaultAsync(m => m.Id == RequestedCampaignItem.CampaignId);

            if (Campaign.SalesPersonId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../Campaigns/Campaigns");
            }
            if (RequestedCampaignItem == null)
            {
                return NotFound();
            }else
            {
                _context.RequestedCampaignItem.Remove(RequestedCampaignItem);
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("Campaign Item Removed!");
            }
            return RedirectToPage("../Campaigns/Details", new { id = campaignId });
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RequestedCampaignItem = await _context.RequestedCampaignItem.FindAsync(id);

            if (RequestedCampaignItem != null)
            {
                _context.RequestedCampaignItem.Remove(RequestedCampaignItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
