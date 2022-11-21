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
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.DemoDetails
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public EditModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        [BindProperty]
        public DemoDetail DemoDetail { get; set; }
        public AppUser AppUser { get; set; }
        public Demo Demo { get; set; }
        public List<Permission> Permissions { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_demodetail");
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
            DemoDetail = await _context.DemoDetail
                .Include(d => d.CropAndAnimal)
                .Include(d => d.PestAndDisease)
                .Include(d => d.CompetingProduct)
                .Include(d => d.Demo)
                .Include(d => d.Product).FirstOrDefaultAsync(m => m.Id == id);
            Demo = await _context.Demo
                .Include(c => c.SalesPerson)
                .Include(u => u.SalesPerson.AppRole)
                .Include(c => c.SalesPerson.UserBusinessLines)
                .Include(c => c.SalesPerson.UserSectors).FirstOrDefaultAsync(u => u.Id == DemoDetail.DemoId);

            
            if (DemoDetail == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_demodetail");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            DemoDetail ApproveDemoDetail = await _context.DemoDetail.Include(d => d.Product).FirstOrDefaultAsync(a => a.Id == DemoDetail.Id);
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).ToListAsync();
            foreach (var userpermission in AppUser.AppRole.AppRolePermissions)
                {
                foreach(var permission in Permissions)
                    {
                    if(permission.Id == userpermission.PermissionId && permission.Name == "foa_approve_demo")
                    {
                        ApproveDemoDetail.IsFOAApproved = true;
                    }
                    if (permission.Id == userpermission.PermissionId && permission.Name == "pd_approve_demo")
                    {
                        ApproveDemoDetail.IsPDApproved = true;
                    }
                }
            }

            ApproveDemoDetail.ApprovedNumberOfDemos = DemoDetail.ApprovedNumberOfDemos;
            ApproveDemoDetail.ApprovedQuantityOfSample = DemoDetail.ApprovedQuantityOfSample;

            Product CurrentProduct = await _context.Product.FirstOrDefaultAsync(p => p.Id == DemoDetail.ProductId);
            ProductCropAndAnimal CurrentCropAndAnimal = await _context.ProductCropAndAnimal.Where(c => c.CropAndAnimalId == DemoDetail.CropAndAnimalId && c.ProductId == DemoDetail.ProductId).FirstOrDefaultAsync();
            DemoDetail.RequestedVolumeOfProduct = ((DemoDetail.RequestedQuantityOfSample * DemoDetail.RequestedNumberOfDemos) / CurrentCropAndAnimal.Servings) * CurrentProduct.PackagingSize;
            _context.Attach(ApproveDemoDetail).State = EntityState.Modified;

            try
            {
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("Details Approved!");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DemoDetailExists(DemoDetail.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Demos/Details", new { id = DemoDetail.DemoId });
        }

        private bool DemoDetailExists(int id)
        {
            return _context.DemoDetail.Any(e => e.Id == id);
        }
    }
}
