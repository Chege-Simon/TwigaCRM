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
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.Sectors
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly CheckPermissionsService _checkPermissions;
        private readonly INotyfService _toastNotification;

        public IndexModel(TwigaCRM.Data.ApplicationDbContext context, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _checkPermissions = checkPermissions;
            _toastNotification = toastNotification;
        }

        public IList<Sector> Sectors { get;set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_sectors");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            Sectors = await _context.Sector.ToListAsync();
            return Page();
        }
        [BindProperty]
        public Sector Sector { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "create_sector");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return Page();
            }

            _context.Sector.Add(Sector);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Sector Created!");

            return RedirectToPage("./Sectors");
        }
    }
}
