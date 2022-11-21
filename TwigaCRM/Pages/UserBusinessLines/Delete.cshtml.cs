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

namespace TwigaCRM.Pages.UserBusinessLines
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
        public UserBusinessLine UserBusinessLine { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id, string userId)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "remove_businessline");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            UserBusinessLine = await _context.UserBusinessLine
                .Include(u => u.AppUser)
                .Include(u => u.BusinessLine).FirstOrDefaultAsync(m => m.Id == id);

            if (UserBusinessLine != null)
            {
                _context.UserBusinessLine.Remove(UserBusinessLine);
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("Business Line Removed!");
            }
            return RedirectToPage("../Users/Details", new {id = userId});
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserBusinessLine = await _context.UserBusinessLine.FindAsync(id);

            if (UserBusinessLine != null)
            {
                _context.UserBusinessLine.Remove(UserBusinessLine);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Users/Users");
        }
    }
}
