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

namespace TwigaCRM.Pages.Campaigns
{
    [Authorize]
    public class CampaignPrintOutModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public CampaignPrintOutModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        [BindProperty]
        public Campaign Campaign { get; set; }
        public AppUser AppUser { get; set; }
        public List<Permission> Permissions { get; set; }
        public IList<RequestedProduct> RequestedProducts { get; set; }
        public IList<RequestedCampaignItem> RequestedCampaignItems { get; set; }
        public IList<RequestedExpense> RequestedExpenses { get; set; }
        [BindProperty]
        public string FOAstatus { get; set; }
        [BindProperty]
        public string HRMstatus { get; set; }
        [BindProperty]
        public decimal CampaignExpenseApprovedAmount { get; set; }
        public string Status { get; set; }
    public bool IsPermitted { get; private set; }

    public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "print_campaign_print_out");
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
            Campaign = await _context.Campaign
                .Include(c => c.CampaignBudget)
                .Include(c => c.CampaignBudget.FinancialYear)
                .Include(c => c.SalesPerson)
                .Include(u => u.SalesPerson.AppRole)
                .Include(c => c.SalesPerson.UserBusinessLines)
                .Include(c => c.SalesPerson.Town)
                .Include(c => c.SalesPerson.Town.Region).FirstOrDefaultAsync(m => m.Id == id);

            RequestedProducts = await _context.RequestedProduct
                .Include(r => r.Campaign)
                .Include(r => r.Product).Where(c => c.Campaign.Id == Campaign.Id).OrderByDescending(s => s.Id).ToListAsync();
            RequestedCampaignItems = await _context.RequestedCampaignItem
                .Include(r => r.Campaign)
                .Include(r => r.CampaignItem).Where(c => c.Campaign.Id == Campaign.Id).OrderByDescending(s => s.Id).ToListAsync();
            RequestedExpenses = await _context.RequestedExpense
                .Include(r => r.Campaign)
                .Include(r => r.ExpenseCategory).Where(c => c.Campaign.Id == Campaign.Id).OrderByDescending(s => s.Id).ToListAsync();
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).OrderByDescending(s => s.Id).ToListAsync();
            FOAstatus = Campaign.FOAstatus;
            HRMstatus = Campaign.HRMstatus;
            Status = Campaign.Status;

            if (Campaign == null)
            {
                return NotFound();
            }
            return Page();
        }

    }
}
