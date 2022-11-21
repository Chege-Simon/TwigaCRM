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

        public IList<SalesMovement> SalesMovements { get;set; }
        public bool IsPermitted { get; private set; }
        public List<Permission> Permissions { get; set; }
        public AppUser AppUser { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_MTs");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            SalesMovements = await _context.SalesMovement
                .Include(s => s.FinancialYear)
                .Include(s => s.SalesPerson).OrderByDescending(s => s.Id).ToListAsync();
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).ToListAsync();

            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
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
        [BindProperty]
        public SalesMovement SalesMovement { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "create_MT");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./SalesMovements");
            }

            SalesMovement NewSalesMovement = await _context.SalesMovement
                .Where(s => s.SalesPersonId == SalesMovement.SalesPersonId && s.Month == SalesMovement.Month && s.FinancialYearId == SalesMovement.FinancialYearId).FirstOrDefaultAsync();
            if (NewSalesMovement != null)
            {
                _toastNotification.Warning("Movement Target Already Exists!!");
                return RedirectToPage("./Details", new { id = NewSalesMovement.Id });
            }
            SalesMovement.CreatorId = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            SalesMovement.IsSubmitted = false;
            SalesMovement.TLstatus = "Pending";
            _context.SalesMovement.Add(SalesMovement);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Movement Target Created!");
            SalesMovement CurrentSalesMovement = await _context.SalesMovement
                .Where(s => s.SalesPersonId == SalesMovement.SalesPersonId && s.Month == SalesMovement.Month && s.FinancialYearId == SalesMovement.FinancialYearId).FirstOrDefaultAsync();

            return RedirectToPage("./Details", new {id = CurrentSalesMovement.Id});
        }
    }
}
