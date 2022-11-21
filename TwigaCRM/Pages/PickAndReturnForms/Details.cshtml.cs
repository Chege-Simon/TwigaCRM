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

namespace TwigaCRM.Pages.PickAndReturnForms
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public DetailsModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        public PickAndReturnForm PickAndReturnForm { get; set; }
        public List<PickAndReturnProduct> PickAndReturnProducts { get; set; }
        public AppUser AppUser { get; set; }
        public bool IsPermitted { get; private set; }
        public List<Permission> Permissions { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_pick_and_return_form_details");
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
            PickAndReturnForm = await _context.PickAndReturnForm
                .Include(p => p.MainDistributor).FirstOrDefaultAsync(m => m.Id == id);

            PickAndReturnProducts = await _context.PickAndReturnProduct
                .Include(p => p.Product)
                .Include(p => p.PickAndReturnForm).Where(p => p.PickAndReturnFormId == PickAndReturnForm.Id).ToListAsync();
            TLstatus = PickAndReturnForm.TLstatus;
            FOAstatus = PickAndReturnForm.FOAstatus;
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).ToListAsync();
            if (PickAndReturnForm == null)
            {
                return NotFound();
            }
            ViewData["Products"] = _context.Product.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name + " (" + a.Description + ") - " + " (" + a.Code + ")"
                                            }).ToList();
            return Page();
        }
        [BindProperty]
        public PickAndReturnProduct PickAndReturnProduct { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_pick_and_return_products");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Details", new { id = PickAndReturnProduct.PickAndReturnFormId });
            }

            PickAndReturnForm = await _context.PickAndReturnForm
                .Include(p => p.MainDistributor).FirstOrDefaultAsync(m => m.Id == PickAndReturnProduct.PickAndReturnFormId);
            if (PickAndReturnForm.VSAId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../PickAndReturnForms/PickAndReturnForms");
            }
            Product currentProduct = await _context.Product.FirstOrDefaultAsync(p => p.Id == PickAndReturnProduct.ProductId);
            var pickedcost = (currentProduct.Price * currentProduct.PackagingSize) * (PickAndReturnProduct.PickedQuantity / currentProduct.PackagingSize);
            var returnedcost = (currentProduct.Price * currentProduct.PackagingSize) * (PickAndReturnProduct.ReturnedQuantity / currentProduct.PackagingSize);
            var pickedvolume = currentProduct.PackagingSize * PickAndReturnProduct.PickedQuantity;
            var returnedvolume = currentProduct.PackagingSize * PickAndReturnProduct.ReturnedQuantity;
            PickAndReturnProduct.PickedVolume = pickedvolume;
            PickAndReturnProduct.PickedValue = pickedcost;
            PickAndReturnProduct.ReturnedVolume = returnedvolume;
            PickAndReturnProduct.ReturnedValue = returnedcost;
            _context.PickAndReturnProduct.Add(PickAndReturnProduct);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Pick & Return Product Recorded!");

            return RedirectToPage("./Details", new { id = PickAndReturnProduct.PickAndReturnFormId });
        }

        [BindProperty]
        public string FOAstatus { get; set; }
        [BindProperty]
        public string TLstatus { get; set; }
        public async Task<IActionResult> OnPostFOAApproveAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "foa_approve_pick_and_return_form");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Details", new { id });
            }
            PickAndReturnForm = await _context.PickAndReturnForm.FirstOrDefaultAsync(d => d.Id == id);

            PickAndReturnForm.IsSubmitted = FOAstatus != "Rejected" ? true : false;
            PickAndReturnForm.FOAstatus = FOAstatus;
            PickAndReturnForm.TLstatus = TLstatus;
            _context.Attach(PickAndReturnForm).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("P & R Approval Changed!");

            return RedirectToPage("./Details", new { id });
        }
        public async Task<IActionResult> OnPostTLApproveAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "tl_approve_pick_and_return_form");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Details", new { id });
            }
            PickAndReturnForm = await _context.PickAndReturnForm.FirstOrDefaultAsync(d => d.Id == id);

            PickAndReturnForm.IsSubmitted = TLstatus != "Rejected" ? true : false;
            PickAndReturnForm.TLstatus = TLstatus;
            PickAndReturnForm.FOAstatus = TLstatus;
            _context.Attach(PickAndReturnForm).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("P & R Approval Changed!");

            return RedirectToPage("./Details", new { id });
        }
    }
}
