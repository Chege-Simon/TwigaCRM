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
using Microsoft.EntityFrameworkCore;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.Demos
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

        public IList<Demo> Demos { get;set; }
        public AppUser AppUser { get; set; }
        public bool IsPermitted { get; private set; }
        public List<Permission> Permissions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_demos");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            Demos = await _context.Demo
                .Include(d => d.SalesPerson).OrderByDescending(s => s.Id).ToListAsync(); 
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).ToListAsync();
            return Page();
        }
        [BindProperty]
        public Demo Demo { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "create_demo");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("invalid Inputs!");
                return RedirectToPage("./Demos");
            }
            Demo.Status = "Open";
            Demo.FOAstatus = "Pending";
            Demo.PDstatus = "Pending";
            _context.Demo.Add(Demo);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Demo Created!");
            Demo CurrentDemo = await _context.Demo
                .Where(s => s.SalesPersonId == Demo.SalesPersonId && s.DemoType == Demo.DemoType && s.StartDate.Date == Demo.StartDate.Date && s.EndDate.Date == Demo.EndDate.Date).FirstOrDefaultAsync();


            return RedirectToPage("./Details", new { id = CurrentDemo.Id });
        }
    }
}
