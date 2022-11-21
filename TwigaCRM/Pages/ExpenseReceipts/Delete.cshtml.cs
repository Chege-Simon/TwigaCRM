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

namespace TwigaCRM.Pages.ExpenseReceipts
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
        public ExpenseReceipt ExpenseReceipt { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ExpenseReceipt = await _context.ExpenseReceipt
                .Include(e => e.RequestedExpense)
                .Include(e => e.RequestedExpense.Campaign).FirstOrDefaultAsync(m => m.Id == id);


            if (ExpenseReceipt != null)
            {
                _context.ExpenseReceipt.Remove(ExpenseReceipt);
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("Receipt Removed!");
            }

            return RedirectToPage("./ExpenseReceipts", new { id = ExpenseReceipt.RequestedExpenseId, campaignId = ExpenseReceipt.RequestedExpense.CampaignId});
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ExpenseReceipt = await _context.ExpenseReceipt.FindAsync(id);

            if (ExpenseReceipt != null)
            {
                _context.ExpenseReceipt.Remove(ExpenseReceipt);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
