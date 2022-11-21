using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.RequestedExpenses
{
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
        public RequestedExpense RequestedExpense { get; set; }
        public List<RequestedExpense> RequestedExpenses { get; set; }
        public List<RequestedCampaignItem> RequestedCampaignItems { get; set; }
        public bool IsPermitted { get; private set; }

        public AppUser AppUser { get; set; }
        public List<Permission> Permissions { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_requested_expense");
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
            RequestedExpense = await _context.RequestedExpense
                .Include(r => r.Campaign)
                .Include(r => r.Campaign.CampaignBudget)
                .Include(r => r.Campaign.CampaignBudget.FinancialYear)
                .Include(r => r.ExpenseCategory).FirstOrDefaultAsync(m => m.Id == id);
            RequestedExpenses = await _context.RequestedExpense
                .Include(r => r.Campaign)
                .Include(r => r.Campaign.CampaignBudget)
                .Include(r => r.Campaign.CampaignBudget.FinancialYear)
                .Include(r => r.ExpenseCategory)
                .Where(e => e.Campaign.CampaignBudgetId == RequestedExpense.Campaign.CampaignBudgetId && e.Campaign.IsSubmitted).ToListAsync();
            RequestedCampaignItems = await _context.RequestedCampaignItem
                .Include(r => r.Campaign)
                .Include(r => r.Campaign.CampaignBudget)
                .Include(r => r.Campaign.CampaignBudget.FinancialYear)
                .Include(r => r.CampaignItem)
                .Where(e => e.Campaign.CampaignBudgetId == RequestedExpense.Campaign.CampaignBudgetId && e.Campaign.IsSubmitted).ToListAsync();
            if (RequestedExpense == null)
            {
                return NotFound();
            }
            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_requested_expense");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            RequestedExpense ApproveExpense = await _context.RequestedExpense.FirstOrDefaultAsync(a => a.Id == RequestedExpense.Id);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("../Campaigns/Details", new { id = ApproveExpense.CampaignId });
            }
            
            ApproveExpense.ApprovedAmount = RequestedExpense.ApprovedAmount;
            ApproveExpense.IsFOAApproved = true;
            _context.Attach(ApproveExpense).State = EntityState.Modified;

            try
            {
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("Campaign Expense Approved!");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestedExpenseExists(RequestedExpense.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Campaigns/Details", new { id = ApproveExpense.CampaignId });
        }

        private bool RequestedExpenseExists(int id)
        {
            return _context.RequestedExpense.Any(e => e.Id == id);
        }
       
    }
}
