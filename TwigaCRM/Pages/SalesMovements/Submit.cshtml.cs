using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TwigaCRM.Data;
using TwigaCRM.Models;

namespace TwigaCRM.Pages.SalesMovements
{
    public class DeleteModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;

        public DeleteModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public SalesMovement SalesMovement { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SalesMovement = await _context.SalesMovement
                .Include(s => s.FinancialYear)
                .Include(s => s.SalesPerson).FirstOrDefaultAsync(m => m.Id == id);

            if (SalesMovement == null)
            {
                return NotFound();
            }


            SalesMovement = await _context.SalesMovement
                .Include(d => d.SalesPerson).FirstOrDefaultAsync(m => m.Id == id);
            if (SalesMovement.CreatorId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../SalesMovements/SalesMovements");
            }
            SalesMovement.IsSubmitted = true;
            SalesMovement.TLstatus = "Pending";
            _context.Attach(SalesMovement).State = EntityState.Modified;
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("M.T Submitted!");

            return RedirectToPage("../SalesMovements/Details", new { id });
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SalesMovement = await _context.SalesMovement.FindAsync(id);

            if (SalesMovement != null)
            {
                _context.SalesMovement.Remove(SalesMovement);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
