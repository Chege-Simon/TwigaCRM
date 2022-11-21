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
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.CompetitionActivities
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly CheckPermissionsService _checkPermissions;
        private readonly INotyfService _toastNotification;

        public EditModel(TwigaCRM.Data.ApplicationDbContext context, INotyfService toastNotification, CheckPermissionsService checkPermissions, UserManager<AppUser> userManager)
        {
            _context = context;
            _checkPermissions = checkPermissions;
            _toastNotification = toastNotification;
            _userManager = userManager;
        }

        [BindProperty]
        public CompetitionActivity CompetitionActivity { get; set; }
        public bool IsPermitted { get; private set; }
        public AppUser AppUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IsPermitted = _checkPermissions.CheckPermission(User, "edit_competition_activity");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            CompetitionActivity = await _context.CompetitionActivity
                .Include(c => c.AppUser).FirstOrDefaultAsync(m => m.Id == id);

            if (CompetitionActivity == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_competition_activity");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            CompetitionActivity.AppUserId = Id;
            _context.Attach(CompetitionActivity).State = EntityState.Modified;

            try
            {
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("Competition Activity Edited!");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompetitionActivityExists(CompetitionActivity.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./CompetitionActivities");
        }

        private bool CompetitionActivityExists(int id)
        {
            return _context.CompetitionActivity.Any(e => e.Id == id);
        }
    }
}
