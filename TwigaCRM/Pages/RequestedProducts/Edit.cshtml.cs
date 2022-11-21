﻿using System;
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

namespace TwigaCRM.Pages.RequestedProducts
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
        public RequestedProduct RequestedProduct { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id, int campaignId)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_requested_product");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            RequestedProduct = await _context.RequestedProduct
                .Include(r => r.Campaign)
                .Include(r => r.Product).FirstOrDefaultAsync(m => m.Id == id);
            RequestedProduct.IsFOAApproved = true;
            _context.Attach(RequestedProduct).State = EntityState.Modified;

            try
            {
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("Campaign Product Approved!");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestedProductExists(RequestedProduct.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("../Campaigns/Details", new { id = campaignId });
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_requested_product");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(RequestedProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestedProductExists(RequestedProduct.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool RequestedProductExists(int id)
        {
            return _context.RequestedProduct.Any(e => e.Id == id);
        }
    }
}
