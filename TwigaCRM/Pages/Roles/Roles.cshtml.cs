using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.Roles
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;


        public IndexModel(TwigaCRM.Data.ApplicationDbContext context, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        public IList<AppRole> AppRoles { get;set; } = default!;
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_roles");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (_context.AppRole != null)
            {
                AppRoles = await _context.AppRole
                    .Include(a => a.AppRolePermissions).ToListAsync();
            }
            return Page();
        }
        [BindProperty]
        public AppRole AppRole { get; set; } = default!;
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "create_roles");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);

            if (!ModelState.IsValid || _context.AppRole == null || AppRole == null)
            {
                _toastNotification.Error("Invalid Input!");
                return Page();
            }

            _context.AppRole.Add(AppRole);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Role Created!");

            return RedirectToPage("./Roles");
        }
    }
}
