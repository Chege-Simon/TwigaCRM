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
using NToastNotify;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.DemoProductRequisition
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;
        private IHostEnvironment _environment;

        public EditModel(TwigaCRM.Data.ApplicationDbContext context, IHostEnvironment environment, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
            _environment = environment;
        }

        [BindProperty]
        public TwigaCRM.Models.DemoProductRequisition DemoProductRequisition { get; set; }
        [BindProperty]
        public IFormFile Upload { get; set; }

        public AppUser AppUser { get; set; }
        public bool IsPermitted { get; private set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_demorequsition");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            DemoProductRequisition = await _context.DemoProductRequisition
                .Include(d => d.Demo)
                .Include(d => d.Product)
                .Include(d => d.Stockist).FirstOrDefaultAsync(m => m.Id == id);
            if (DemoProductRequisition == null)
            {
                return NotFound();
            }
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            ViewData["DemoProducts"] = _context.DemoDetail.Include(c => c.Product).Where(c => c.DemoId == DemoProductRequisition.Demo.Id).Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Product.Id.ToString(),
                                                Text = a.Product.Description
                                            }).ToList();
            ViewData["Stockists"] = _context.Customer.Include(c => c.Town).Where(c => c.CustomerType.Contains("Stockist")).Where(c => c.Town.Id == AppUser.Town.Id).Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.SiteName
                                            }).ToList();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_demorequsition");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("..Demos/Details", new { id = DemoProductRequisition.DemoId });
            }
            if (Upload.FileName != null)
            {
                var fileName = DateTime.Now.Ticks + Upload.FileName;
                System.IO.Directory.CreateDirectory("uploads");
                var file = Path.Combine(_environment.ContentRootPath, "uploads", fileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await Upload.CopyToAsync(fileStream);
                }
                DemoProductRequisition.InvoiceUrl = fileName;
            }
            _context.Attach(DemoProductRequisition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DemoProductRequisitionExists(DemoProductRequisition.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DemoProductRequisitionExists(int id)
        {
            return _context.DemoProductRequisition.Any(e => e.Id == id);
        }
    }
}
