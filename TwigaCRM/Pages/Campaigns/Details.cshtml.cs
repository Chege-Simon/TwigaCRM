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
    public class DetailsModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public DetailsModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        public AppUser AppUser { get; set; }
        public List<Permission> Permissions { get; set; }
        public IList<RequestedProduct> RequestedProducts { get; set; }
        public IList<RequestedCampaignItem> RequestedCampaignItems { get; set; }
        public IList<RequestedExpense> RequestedExpenses { get; set; }
        public Campaign Campaign { get; set; }
        [BindProperty]
        public string FOAstatus { get; set; }
        [BindProperty]
        public string HRMstatus { get; set; }
        [BindProperty]
        public decimal CampaignExpenseApprovedAmount { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_campaign_details");
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
                .Include(r => r.Product).Where(r => r.Campaign.Id == Campaign.Id).OrderByDescending(s => s.Id).ToListAsync();
            RequestedCampaignItems = await _context.RequestedCampaignItem
                .Include(r => r.Campaign)
                .Include(r => r.CampaignItem).Where(r => r.Campaign.Id == Campaign.Id).OrderByDescending(s => s.Id).ToListAsync();
            RequestedExpenses = await _context.RequestedExpense
                .Include(r => r.Campaign)
                .Include(r => r.ExpenseCategory).Where(r => r.Campaign.Id == Campaign.Id).OrderByDescending(s => s.Id).ToListAsync();
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).OrderByDescending(s => s.Id).ToListAsync();
            FOAstatus = Campaign.FOAstatus;
            HRMstatus = Campaign.HRMstatus;
            Status = Campaign.Status;
            List<Product> AllProducts = _context.Product
                .Include(t => t.BusinessLine)
                .Include(t => t.ProductsCropsAndAnimals).ToList();
            List<Product> SelectableProducts = new() { };
            foreach (var product in AllProducts)
            {
                foreach (var businessline in Campaign.SalesPerson.UserBusinessLines)
                {
                    if (product.BusinessLineId == businessline.BusinessLineId)
                    {
                        SelectableProducts.Add(product);
                    }
                }
            }
            ViewData["Products"] = SelectableProducts.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name + " (" + a.Description + ") - " + " (" + a.Code + ") - " + a.BusinessLine.NormalizedName
                                            }).ToList();
            ViewData["Items"] = _context.CampaignItem.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name + " - " + a.Description
                                            }).ToList();
            ViewData["Expenses"] = _context.ExpenseCategory.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name + " - " + a.Description
                                            }).ToList();
            if (Campaign == null)
            {
                return NotFound();
            }
            return Page();
        }
        [BindProperty]
        public RequestedProduct RequestedProduct { get; set; }
        public RequestedProduct CampaignRequestedProduct { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAddProductAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_requested_campaign_product");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Details", new { id });
            }
            CampaignRequestedProduct = await _context.RequestedProduct.Where(c => c.CampaignId == id && c.ProductId == RequestedProduct.ProductId).FirstOrDefaultAsync();
            if(CampaignRequestedProduct != null)
            {
                _toastNotification.Information("Product Already Added!");
                return RedirectToPage("./Details", new { id });
            }
            Campaign = await _context.Campaign
                .Include(c => c.CampaignBudget)
                .Include(c => c.CampaignBudget.FinancialYear)
                .Include(c => c.SalesPerson)
                .Include(c => c.SalesPerson.Town)
                .Include(c => c.SalesPerson.Town.Region).FirstOrDefaultAsync(m => m.Id == id);
            if (Campaign.SalesPersonId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../Campaigns/Campaigns");
            }
            Product CurrentProduct = await _context.Product.FirstOrDefaultAsync(p => p.Id == RequestedProduct.ProductId);
            RequestedProduct.CurrentMovementValue = RequestedProduct.CurrentMovement * CurrentProduct.Price;
            RequestedProduct.ProjectedMovementValue = RequestedProduct.ProjectedMovement * CurrentProduct.Price;

            _context.RequestedProduct.Add(RequestedProduct);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Product Added!");

            return RedirectToPage("./Details", new { id = RequestedProduct.CampaignId });
        }
        [BindProperty]
        public RequestedCampaignItem RequestedCampaignItem { get; set; }
        public RequestedCampaignItem CampaignRequestedCampaignItem { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAddCampaignItemAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_requested_campaign_item");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Details", new { id });
            }
            CampaignRequestedCampaignItem = await _context.RequestedCampaignItem.Where(c => c.CampaignId == id && c.CampaignItemId == RequestedCampaignItem.CampaignItemId).FirstOrDefaultAsync();
            if (CampaignRequestedCampaignItem != null)
            {
                _toastNotification.Information("Campaign Item Already Added!");
                return RedirectToPage("./Details", new { id });
            }

            Campaign = await _context.Campaign
                .Include(c => c.SalesPerson).FirstOrDefaultAsync(m => m.Id == id);
            if (Campaign.SalesPersonId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../Campaigns/Campaigns");
            }
            _context.RequestedCampaignItem.Add(RequestedCampaignItem);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Campaign Item Added!");

            return RedirectToPage("./Details", new { id }); ;
        }
        [BindProperty]
        public RequestedExpense RequestedExpense { get; set; }
        public RequestedExpense CampaignRequestedExpense { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAddExpenseAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_requested_campaign_expense");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Details", new { id });
            }
            CampaignRequestedExpense = await _context.RequestedExpense.Where(c => c.CampaignId == id && c.ExpenseCategoryId == RequestedExpense.ExpenseCategoryId).FirstOrDefaultAsync();
            if (CampaignRequestedExpense != null)
            {
                _toastNotification.Information("Expense Already Added!");
                return RedirectToPage("./Details", new { id });
            }

            Campaign = await _context.Campaign
                .Include(c => c.SalesPerson).FirstOrDefaultAsync(m => m.Id == id);
            if (Campaign.SalesPersonId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../Campaigns/Campaigns");
            }
            _context.RequestedExpense.Add(RequestedExpense);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Expense Added!");

            return RedirectToPage("./Details", new { id }); ;
        }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostFOAApproveAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "foa_approve_campaign");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Details", new { id });
            }
            Campaign = await _context.Campaign.FirstOrDefaultAsync(d => d.Id == id);

            List<RequestedCampaignItem> reqItems = await _context.RequestedCampaignItem
                .Include(r => r.Campaign)
                .Include(r => r.CampaignItem)
                .Where(r => r.CampaignId == Campaign.Id).ToListAsync();
            foreach (var item in reqItems)
            {
                if (!item.IsFOAApproved)
                {
                    _toastNotification.Information("Item: " + item.CampaignItem.Name + " NOT APPROVED!");
                    return RedirectToPage("./Details", new { id });
                }
            }
            List<RequestedExpense> reqExpenses = await _context.RequestedExpense
                .Include(r => r.Campaign)
                .Include(r => r.ExpenseCategory)
                .Where(r => r.CampaignId == Campaign.Id).ToListAsync();
            foreach (var item in reqExpenses)
            {
                if (!item.IsFOAApproved)
                {
                    _toastNotification.Information("Expense: " + item.ExpenseCategory.Name + " NOT APPROVED!");
            return RedirectToPage("./Details", new { id });
                }
            }
            List<RequestedProduct> reqProducts = await _context.RequestedProduct
                .Include(r => r.Campaign)
                .Include(r => r.Product)
                .Where(r => r.CampaignId == Campaign.Id).ToListAsync();
            foreach (var item in reqProducts)
            {
                if (!item.IsFOAApproved)
                {
                    _toastNotification.Information("Product: " + item.Product.Name+ "("+ item.Product.Code +")" + " NOT APPROVED!");
            return RedirectToPage("./Details", new { id });
                }
            }
            Campaign.IsSubmitted = FOAstatus != "Rejected" ? true : false;
            Campaign.HRMstatus = Campaign.FOAstatus != FOAstatus ? "Pending" : Campaign.HRMstatus;
            Campaign.FOAstatus = FOAstatus;
            _context.Attach(Campaign).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Campaign Approval Changed!");

            return RedirectToPage("./Details", new { id });
        }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostHRMApproveAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "hrm_approve_campaign");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Details", new { id });
            }

            Campaign = await _context.Campaign.FirstOrDefaultAsync(d => d.Id == id);

            Campaign.IsSubmitted = HRMstatus != "Rejected" ? true : false;
            Campaign.HRMstatus = HRMstatus;
            _context.Attach(Campaign).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Campaign Approval Changed!");

            return RedirectToPage("./Details", new { id });
        }
        [BindProperty]
        public string Remarks { get; set; }
        public async Task<IActionResult> OnPostAddRemarksAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_campaign_remarks");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Details", new { id });
            }

            Campaign = await _context.Campaign.FirstOrDefaultAsync(d => d.Id == id);
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);

            Campaign.Remarks = Campaign.Remarks + System.Environment.NewLine + AppUser.FirstName + " " + AppUser.LastName + " ( " + AppUser.AppRole.Title + ") : " + Remarks;
            _context.Attach(Campaign).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Remarks Added!");

            return RedirectToPage("./Details", new { id });
        }
        [BindProperty]
        public string Status { get; set; }
        public async Task<IActionResult> OnPostCampaignStatusAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "change_campaign_status");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Details", new { id });
            }

            Campaign = await _context.Campaign.FirstOrDefaultAsync(d => d.Id == id);

            Campaign.Status = Status;
            _context.Attach(Campaign).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Campaign Status Changed!");

            return RedirectToPage("./Details", new { id });
        }

    }
}
