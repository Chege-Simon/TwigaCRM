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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.RequestedCampaignItems
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public EditModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        [BindProperty]
        public RequestedCampaignItem RequestedCampaignItem { get; set; }
        public List<RequestedExpense> RequestedExpenses { get; set; }
        public List<RequestedCampaignItem> RequestedCampaignItems { get; set; }
        public bool IsPermitted { get; private set; }
        public AppUser AppUser { get; set; }
        public List<Permission> Permissions { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_requested_campaign_item");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).ToListAsync();
            RequestedCampaignItem = await _context.RequestedCampaignItem
                .Include(r => r.Campaign)
                .Include(r => r.CampaignItem).FirstOrDefaultAsync(m => m.Id == id);

            RequestedExpenses = await _context.RequestedExpense
                .Include(r => r.Campaign)
                .Include(r => r.Campaign.CampaignBudget)
                .Include(r => r.Campaign.CampaignBudget.FinancialYear)
                .Include(r => r.ExpenseCategory)
                .Where(e => e.Campaign.CampaignBudgetId == RequestedCampaignItem.Campaign.CampaignBudgetId && e.Campaign.IsSubmitted).ToListAsync();
            RequestedCampaignItems = await _context.RequestedCampaignItem
                .Include(r => r.Campaign)
                .Include(r => r.Campaign.CampaignBudget)
                .Include(r => r.Campaign.CampaignBudget.FinancialYear)
                .Include(r => r.CampaignItem)
                .Where(e => e.Campaign.CampaignBudgetId == RequestedCampaignItem.Campaign.CampaignBudgetId && e.Campaign.IsSubmitted).ToListAsync();
            if (RequestedCampaignItem == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_requested_campaign_item");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            RequestedCampaignItem ApproveItem = await _context.RequestedCampaignItem.Include(a => a.CampaignItem).FirstOrDefaultAsync(a => a.Id == RequestedCampaignItem.Id);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("../Campaigns/Details", new { id = ApproveItem.CampaignId });
            }

            ApproveItem.FOAApprovedQuantity = RequestedCampaignItem.FOAApprovedQuantity;
            ApproveItem.FOAApprovedPrice = ApproveItem.CampaignItem.Price * RequestedCampaignItem.FOAApprovedQuantity;
            ApproveItem.IsFOAApproved = true;
            _context.Attach(ApproveItem).State = EntityState.Modified;

            try
            {
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("Campaign Item Approved!");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestedCampaignItemExists(RequestedCampaignItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Campaigns/Details", new { id = ApproveItem.CampaignId });
        }

        private bool RequestedCampaignItemExists(int id)
        {
            return _context.RequestedCampaignItem.Any(e => e.Id == id);
        }
    }
}
