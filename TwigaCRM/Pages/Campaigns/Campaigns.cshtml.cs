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

namespace TwigaCRM.Pages.Campaigns
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public IndexModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        public IList<Campaign> Campaigns { get;set; }
        public AppUser AppUser { get; set; }
        public bool IsPermitted { get; private set; }
        public List<Permission> Permissions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_campaigns");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.UserBusinessLines)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).ToListAsync();
            Campaigns = await _context.Campaign
                .Include(c => c.CampaignBudget)
                .Include(c => c.SalesPerson).OrderByDescending(s => s.Id).ToListAsync();
            ViewData["Budgets"] = _context.CampaignBudget
                .Include(t => t.FinancialYear)
                .Include(t => t.BusinessLine)
                .Where(t => t.Month == DateTime.Now.Month).Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Description+" ( "+ a.BusinessLine.NormalizedName+" )" + " - " + a.FinancialYear.StartDate.Date + " - " + a.FinancialYear.EndDate.Date
                                            }).ToList();
            ViewData["Stockists"] = _context.Customer
                .Include(t => t.Town)
                .Include(t => t.Town.Zones)
                .Include(t => t.Town.Region)
                .Where(t => t.TownId == AppUser.TownId).Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.SiteName + " - " + a.ContactPersonName
                                            }).ToList();
            return Page();
        }
        [BindProperty]
        public Campaign Campaign { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "create_campaign");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("invalid Inputs!");
                return RedirectToPage("./Campaigns");
            }
            Campaign.IsBudgeted = Campaign.CampaignBudgetId != null ? true : false;
            Campaign.Status = "Open";
            Campaign.FOAstatus = "Pending";
            Campaign.HRMstatus = "Pending";
            _context.Campaign.Add(Campaign);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Campaign Created!");
            Campaign CurrentCampaign = await _context.Campaign
                .Where(s => s.SalesPersonId == Campaign.SalesPersonId && s.CampaignType == Campaign.CampaignType && s.StartDate.Date == Campaign.StartDate.Date && s.EndDate.Date == Campaign.EndDate.Date).FirstOrDefaultAsync();


            return RedirectToPage("./Details", new {id = CurrentCampaign.Id});
        }
    }
}
