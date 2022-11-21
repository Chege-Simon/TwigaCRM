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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.PickAndReturnForms
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;
        private IHostEnvironment _environment;

        public IndexModel(TwigaCRM.Data.ApplicationDbContext context, IHostEnvironment environment, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
            _environment = environment;
        }

        public IList<PickAndReturnForm> PickAndReturnForms { get;set; }
        public bool IsPermitted { get; private set; }
        public AppUser AppUser { get; set; }
        public List<Permission> Permissions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_pick_and_return_forms");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            PickAndReturnForms = await _context.PickAndReturnForm
                .Include(p => p.MainDistributor).ToListAsync();
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).ToListAsync();
            ViewData["MainDistributors"] = _context.Customer.Include(t => t.Town)
                                            .Include(t => t.Town.Region)
                                                .Where(t => t.CustomerType == "Main Distributor").Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.SiteName + " - " + a.ContactPersonName
                                            }).ToList();
            return Page();
        }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        [BindProperty]
        public PickAndReturnForm PickAndReturnForm { get; set; }
        [BindProperty]
        public IFormFile Upload { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "create_pick_and_return_form");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./PickAndReturnForms");
            }
            var UserId = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            var CurrentPickAndReturnForm = await _context.PickAndReturnForm.Where(d => d.VSAId == UserId && d.PickDate.Date == PickAndReturnForm.PickDate.Date).FirstOrDefaultAsync();
            if (CurrentPickAndReturnForm != null)
            {
                _toastNotification.Warning("P & R Already Exists!");
                return RedirectToPage("./Details", new { id = CurrentPickAndReturnForm.Id });
            }
            var fileName = DateTime.Now.Ticks + Upload.FileName;
            System.IO.Directory.CreateDirectory("uploads");
            var file = Path.Combine(_environment.ContentRootPath, "uploads", fileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }
            PickAndReturnForm.FormUrl = fileName;
            PickAndReturnForm.IsSubmitted = false;
            PickAndReturnForm.FOAstatus = "Pending";
            PickAndReturnForm.TLstatus = "Pending";
            _context.PickAndReturnForm.Add(PickAndReturnForm);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Pick And Return Form Created!");
            CurrentPickAndReturnForm = await _context.PickAndReturnForm.Where(d => d.VSAId == UserId && d.PickDate.Date == PickAndReturnForm.PickDate.Date).FirstOrDefaultAsync();

            return RedirectToPage("./Details", new { id = CurrentPickAndReturnForm.Id });
        }
        public FileResult OnGetDownloadFile(string fileName)
        {
            //Build the File Path.
            string path = Path.Combine(_environment.ContentRootPath, "uploads/") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            _toastNotification.Success("Form Downloaded!");
            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }
    }
}
