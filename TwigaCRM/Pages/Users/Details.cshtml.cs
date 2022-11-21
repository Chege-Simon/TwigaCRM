using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using AspNetCoreHero.ToastNotification.Abstractions;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Areas.Identity.Pages.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TwigaCRM.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TwigaCRM.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly CheckPermissionsService _checkPermissions;
        private readonly INotyfService _toastNotification;

        public DetailsModel(ApplicationDbContext context,
            UserManager<AppUser> userManager,
            ILogger<RegisterModel> logger,
            INotyfService toastNotification, 
            CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _checkPermissions = checkPermissions;
            _toastNotification = toastNotification;
        }
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        [BindProperty]
        public AppUser AppUser { get; set; }
        public string UserRoleId { get; set; }
        public AppRole UserRole { get; set; }

        public List<AppRole> AppRoles { get; set; }
        public List<BusinessLine> BusinessLines { get; set; }
        public List<Sector> Sectors { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_user_details");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            Sectors = await _context.Sector.Include(s => s.UserSectors).ToListAsync();
            BusinessLines = await _context.BusinessLine.Include(b => b.UserBusinessLines).ToListAsync();
            AppUser = await _userManager.Users.Include(u => u.UserBusinessLines)
                .Include(b => b.Town)
                .Include(b => b.Town.Region)
                .Include(u => u.UserSectors).FirstOrDefaultAsync(u => u.Id == id);
            //AppUser = editUser;
            UserRoleId = AppUser.UserAppRoleId.ToString();
            var userRole = await _context.AppRole.FindAsync(Int32.Parse(UserRoleId));
            UserRole = userRole;

            ViewData["BusinessLines"] = _context.BusinessLine.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.NormalizedName + " - " + a.Description
                                            }).ToList();
            ViewData["Sectors"] = _context.Sector.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.NormalizedName + " - " + a.Description
                                            }).ToList();
            return Page();

        }
        [BindProperty]
        public UserBusinessLine UserBusinessLine { get; set; }
        public List<UserBusinessLine> UserBusinessLines { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAssignBusinessLineAsync(string id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "assign_user_businessline");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            UserBusinessLines = await _context.UserBusinessLine.ToListAsync();
            foreach (var userbusinessline in UserBusinessLines)
            {
                if (userbusinessline.BusinessLineId == UserBusinessLine.BusinessLineId && userbusinessline.AppUserId == UserBusinessLine.AppUserId)
                {
                    _toastNotification.Information("Business Line Already Assigned!");
                    return RedirectToPage("./Details", new { id });
                }
            }
            _context.UserBusinessLine.Add(UserBusinessLine);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Business Line Assigned!");

            return RedirectToPage("./Details", new { id });
        }

        [BindProperty]
        public UserSector UserSector { get; set; }
        public List<UserSector> UserSectors { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAssignSectorAsync(string id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "assign_user_sector");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            UserSectors = await _context.UserSector.ToListAsync();
            foreach (var usersector in UserSectors)
            {
                if (usersector.SectorId == UserSector.SectorId && usersector.AppUserId == UserSector.AppUserId)
                {
                    _toastNotification.Information("Sector Already Assigned!");
                    return RedirectToPage("./Details", new { id });
                }
            }
            _context.UserSector.Add(UserSector);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Sector Assigned!");

            return RedirectToPage("./Details", new { id });
        }
    }
}
