using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.Roles
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly CheckPermissionsService _checkPermissions;
        private readonly INotyfService _toastNotification;

        public DetailsModel(TwigaCRM.Data.ApplicationDbContext context, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _checkPermissions = checkPermissions;
            _toastNotification = toastNotification;
        }

        public IList<AppRole_Permission> AppRolePermissions { get; set; } = default!;
        public IList<Permission> Permissions { get; set; }
        public AppRole AppRole { get; set; }
        [BindProperty]
        public AppRole_Permission AppRolePermission { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_role_details");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null || _context.AppRole_Permission == null)
            {
                return NotFound();
            }

            var approle_permission = await _context.AppRole_Permission.Where(m => m.AppRoleId == id).ToListAsync();
            if (approle_permission == null)
            {
                return NotFound();
            }
            else
            {
                AppRolePermissions = approle_permission;
                AppRole = await _context.AppRole.Include(a => a.AppRolePermissions).FirstOrDefaultAsync(m => m.Id == id);
                Permissions = await _context.Permission.ToListAsync();
            }

            ViewData["AppRoles"] = _context.AppRole.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Title
                                            }).ToList();
            ViewData["Permissions"] = _context.Permission.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name + " : " + a.Description
                                            }).ToList();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "assign_permission");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid || _context.AppRole_Permission == null || AppRolePermission == null)
            {
                return Page();
            }
            if (AppRole_PermissionExistsAndActive(AppRolePermission.AppRoleId, AppRolePermission.PermissionId))
            {
                _toastNotification.Warning("Permission Already Assigned!");
                return RedirectToPage("./Details", new { id = AppRolePermission.AppRoleId });
            }
            _context.AppRole_Permission.Add(AppRolePermission);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Permissions Assigned!");
            return RedirectToPage("./Details", new {id = AppRolePermission.AppRoleId});
        }

        public async Task<IActionResult> OnPostAssignPermissionAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "assign_permission");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid || _context.AppRole_Permission == null || AppRolePermission == null)
            {
                return Page();
            }
            if (AppRole_PermissionExistsAndActive(AppRolePermission.AppRoleId, AppRolePermission.PermissionId))
            {
                _toastNotification.Warning("Permission Already Assigned!");
                return RedirectToPage("./Details", new { id = AppRolePermission.AppRoleId });
            }
            _context.AppRole_Permission.Add(AppRolePermission);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Permissions Assigned!");
            return RedirectToPage("./Details", new { id = AppRolePermission.AppRoleId });
        }
        private bool AppRole_PermissionExistsAndActive(int roleId, int permissionId)
        {
            return (_context.AppRole_Permission?.Any(e => e.AppRoleId == roleId && e.PermissionId == permissionId)).GetValueOrDefault();
        }

        [BindProperty]
        public AppRole_Permission EditAppRolePermission { get; set; }

        public async Task<IActionResult> OnPostRemovePermissionAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "disable_permission");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Details", new { id = EditAppRolePermission.AppRoleId });
            }

            if (AppRole_PermissionExistsAndActive(EditAppRolePermission.AppRoleId, EditAppRolePermission.PermissionId))
            {
                AppRole_Permission DisabledAppRolePermission = await _context.AppRole_Permission.Where(a => a.AppRoleId == EditAppRolePermission.AppRoleId && a.PermissionId == EditAppRolePermission.PermissionId).FirstOrDefaultAsync();
                _context.AppRole_Permission.Remove(DisabledAppRolePermission);

                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("Permissions Disabled!");
                return RedirectToPage("./Details", new { id = EditAppRolePermission.AppRoleId });
            }
                _toastNotification.Warning("Permission Already Disabled!");
                return RedirectToPage("./Details", new { id = EditAppRolePermission.AppRoleId });

        }

        [BindProperty]
        public AppRole_Permission AssignAppRolePermission { get; set; }
        public AppRole_Permission NewAppRolePermission { get; set; }

        public async Task<IActionResult> OnPostAssignAllPermissionsAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "assign_all_permission");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            foreach (var permission in await _context.Permission.ToListAsync())
            {

                NewAppRolePermission = new()
                {
                    PermissionId = permission.Id,
                    AppRoleId = AssignAppRolePermission.AppRoleId,
                };
                if (await _context.AppRole_Permission.Where(p => p.PermissionId == NewAppRolePermission.PermissionId && p.AppRoleId == NewAppRolePermission.AppRoleId).FirstOrDefaultAsync() == null)
                {
                    _context.AppRole_Permission.Add(NewAppRolePermission);
                    await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
            }
            _toastNotification.Success("Assigned All Permissions!");
            return RedirectToPage("./Details", new { id = AssignAppRolePermission.AppRoleId });

        }
    }
   
}
