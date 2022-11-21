using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Pages.Users;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.Permissions
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly CheckPermissionsService _checkPermissions;
        private readonly INotyfService _toastNotification;
        private IHostEnvironment _environment;
        private readonly UserManager<AppUser> _userManager;

        public IndexModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions, IHostEnvironment environment)
        {
            _context = context;
            _checkPermissions = checkPermissions;
            _toastNotification = toastNotification;
            _environment = environment;
            _userManager = userManager;
        }

        public IList<Permission> Permissions { get;set; } = default!;
        public bool IsPermitted { get; private set; }
        public AppUser AppUser { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_permissions");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);

            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            if (_context.Permission != null)
            {
                Permissions = await _context.Permission.ToListAsync();
            }
            return Page();
        }
        [BindProperty]
        public Permission Permission { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "create_permissions");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid || _context.Permission == null || Permission == null)
            {
                _toastNotification.Error("Invalid Inputs!");

                return RedirectToPage("./Permissions");
            }

            _context.Permission.Add(Permission);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Permissions Created!");

            return RedirectToPage("./Permissions");
        }

        [BindProperty]
        public IFormFile Upload { get; set; }
        public List<Permission> NewPermissions { get; set; }
        public async Task<IActionResult> OnPostImportAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "import_permissions");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            var fileName = DateTime.Now.Ticks + Upload.FileName;
            System.IO.Directory.CreateDirectory("uploads");
            var file = Path.Combine(_environment.ContentRootPath, "uploads", fileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }
            NewPermissions = new List<Permission>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(file, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        Permission NewPermission = new Permission();
                        while (reader.Read()) //Each ROW
                        {
                            for (int column = 0; column < reader.FieldCount; column++)
                            {
                                if (column == 0)
                                {
                                    continue;
                                }
                                //Console.WriteLine(reader.GetString(column));//Will blow up if the value is decimal etc. 
                                //Console.WriteLine(reader.GetValue(column));//Get Value returns object
                                NewPermission = new()
                                {
                                    Name = reader.GetValue(0).ToString(),
                                    Description = reader.GetValue(1).ToString(),
                                };
                            }
                            if(await _context.Permission.Where(p => p.Name == NewPermission.Name).FirstOrDefaultAsync() == null)
                            {
                                _context.Permission.Add(NewPermission);
                                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                            }
                        }
                    } while (reader.NextResult()); //Move to NEXT SHEET

                }
            }
            //foreach (var permission in Permissions)
            //{
            //    Permission NewPermission = new Permission();
            //    _context.Permission.Add(permission);
            //    await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            //}
            _toastNotification.Success("Permissions Added!");

            return RedirectToPage("./Permissions");
        }
    }
}
