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

namespace TwigaCRM.Pages.SalesMovements
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
        public SalesMovement SalesMovement { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_MT");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
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
            ViewData["FinancialYears"] = _context.FinancialYear.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.StartDate.Year + " - " + a.EndDate.Year
                                            }).ToList();
            ViewData["SalesPersons"] = _userManager.Users.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.FirstName + " " + a.LastName
                                            }).ToList();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_MT");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Edit", new {id = SalesMovement.Id});
            }

            if (SalesMovement.CreatorId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../SalesMovements/SalesMovements");
            }
            SalesMovement.TLstatus = "Pending";
            _context.Attach(SalesMovement).State = EntityState.Modified;

            try
            {
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("Movement Target Edited!");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesMovementExists(SalesMovement.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./SalesMovements");
        }

        private bool SalesMovementExists(int id)
        {
            return _context.SalesMovement.Any(e => e.Id == id);
        }
    }
}
