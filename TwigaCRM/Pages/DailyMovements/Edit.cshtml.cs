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

namespace TwigaCRM.Pages.DailyMovements
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly CheckPermissionsService _checkPermissions;
        private readonly INotyfService _toastNotification;
        private readonly UserManager<AppUser> _userManager;

        public EditModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _checkPermissions = checkPermissions;
            _toastNotification = toastNotification;
            _userManager = userManager;
        }

        [BindProperty]
        public DailyMovement DailyMovement { get; set; }
        public AppUser AppUser { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_daily_movement");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            DailyMovement = await _context.DailyMovement
                .Include(d => d.MainDistributor)
                .Include(d => d.Product)
                .Include(d => d.DailyMovementReport).FirstOrDefaultAsync(m => m.Id == id);
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);

            if (DailyMovement == null)
            {
                return NotFound();
            }
            if (AppUser.Town != null)
            {
                ViewData["MainDistributors"] = _context.Customer.Include(t => t.Town).Include(t => t.Town.Region)
                                                .Where(t => t.Town.Region.Id == AppUser.Town.Region.Id)
                                                .Where(t => t.CustomerType == "Main Distributor").Select(a =>
                                                new SelectListItem
                                                {
                                                    Value = a.Id.ToString(),
                                                    Text = a.SiteName + " - " + a.ContactPersonName
                                                }).ToList();
            }
            else
            {
                ViewData["MainDistributors"] = _context.Customer.Include(t => t.Town)
                                                .Include(t => t.Town.Region)
                                                    .Where(t => t.CustomerType == "Main Distributor").Select(a =>
                                                new SelectListItem
                                                {
                                                    Value = a.Id.ToString(),
                                                    Text = a.SiteName + " - " + a.ContactPersonName
                                                }).ToList();
            }
            ViewData["Products"] = _context.Product.Include(t => t.BusinessLine).Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name + " (" + a.Code + ") - " + a.BusinessLine.NormalizedName
                                            }).ToList();
            ViewData["StokistOrFarmers"] = _context.Customer.Include(t => t.Town).Include(t => t.Town.Region).Where(t => t.Town.Region.Id == AppUser.Town.Region.Id).Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.SiteName + " - " + a.ContactPersonName
                                            }).ToList();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_daily_movement");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./DailyMovements");
            }
            var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == DailyMovement.ProductId);
            
            var cost = product.Price * DailyMovement.Quantity;
            DailyMovement.TotalAmount = cost;
            _context.Attach(DailyMovement).State = EntityState.Modified;

            try
            {
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("Movement Edited!");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DailyMovementExists(DailyMovement.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new {id = DailyMovement.DailyMovementReportId});
        }

        private bool DailyMovementExists(int id)
        {
            return _context.DailyMovement.Any(e => e.Id == id);
        }
    }
}
