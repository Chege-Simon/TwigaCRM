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
using Microsoft.Extensions.Hosting;
using NToastNotify;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.ExpenseReceipts
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private IHostEnvironment _environment;
        private readonly CheckPermissionsService _checkPermissions;
        private readonly INotyfService _toastNotification;

        public IndexModel(TwigaCRM.Data.ApplicationDbContext context, IHostEnvironment environment, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _environment = environment;
            _checkPermissions = checkPermissions;
            _toastNotification = toastNotification;
        }

        public IList<ExpenseReceipt> ExpenseReceipts { get;set; }
        public RequestedExpense Expense { get; set; }
        [BindProperty]
        public ExpenseReceipt ExpenseReceipt { get; set; }
        [BindProperty]
        public int CampaignId { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int id, int campaignId)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "campaign_receipts");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            CampaignId = campaignId;
            ExpenseReceipt = new ExpenseReceipt();
            Expense = await _context.RequestedExpense.FirstOrDefaultAsync(e => e.Id == id);
            ExpenseReceipt.RequestedExpenseId = Expense.Id;
            ExpenseReceipts = await _context.ExpenseReceipt
                .Include(e => e.RequestedExpense)
                .Include(e => e.RequestedExpense.Campaign)
                .Include(e => e.RequestedExpense.ExpenseCategory)
                .Where(e => e.RequestedExpenseId == id).ToListAsync();
            return Page();
        }


        [BindProperty]
        public IFormFile Upload { get; set; }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            Campaign Campaign = await _context.Campaign
                .Include(c => c.SalesPerson).FirstOrDefaultAsync(m => m.Id == CampaignId);
            if (Campaign.SalesPersonId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../Campaigns/Campaigns");
            }
            var fileName = DateTime.Now.Ticks + Upload.FileName;
            System.IO.Directory.CreateDirectory("uploads");
            var file = Path.Combine(_environment.ContentRootPath, "uploads", fileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }
            ExpenseReceipt.DocumentUrl = fileName;
            _context.ExpenseReceipt.Add(ExpenseReceipt);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Receipt Added!");

            return RedirectToPage("./ExpenseReceipts", new { id = ExpenseReceipt.RequestedExpenseId, campaignId = Campaign.Id });
        }
        public FileResult OnGetDownloadFile(string fileName)
        {
            //Build the File Path.
            string path = Path.Combine(_environment.ContentRootPath, "uploads/") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            _toastNotification.Success("Receipt Downloaded!");
            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }
    }
}
