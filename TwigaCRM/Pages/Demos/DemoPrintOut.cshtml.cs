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
using NToastNotify;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.Demos
{
    [Authorize]
    public class DemoPrintOutModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public DemoPrintOutModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        public AppUser AppUser { get; set; }
        public List<Permission> Permissions { get; set; }
        public IList<DemoDetail> DemoDetails { get; set; }
        public Demo Demo { get; set; }
        [BindProperty]
        public string FOAstatus { get; set; }
        [BindProperty]
        public string PDstatus { get; set; }
        public string Status { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "print_demo_print_out");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            Demo = await _context.Demo
                .Include(c => c.SalesPerson)
                .Include(u => u.SalesPerson.AppRole)
                .Include(c => c.SalesPerson.UserBusinessLines)
                .Include(c => c.SalesPerson.UserSectors).FirstOrDefaultAsync(u => u.Id == id);
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            FOAstatus = Demo.FOAstatus;
            PDstatus = Demo.PDstatus;
            Status = Demo.Status;
            DemoDetails = await _context.DemoDetail
                .Include(r => r.Demo)
                .Include(r => r.Product)
                .Include(r => r.DemoProgressReports)
                .Include(r => r.PestAndDisease)
                .Include(r => r.CropAndAnimal)
                .Include(r => r.CompetingProduct).Where(d => d.Demo.Id == Demo.Id).OrderByDescending(s => s.Id).ToListAsync();

            if (Demo == null)
            {
                return NotFound();
            }
            return Page();
        }

    }
}
