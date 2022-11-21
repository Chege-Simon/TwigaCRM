using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.ProductsCropsAndAnimals
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly CheckPermissionsService _checkPermissions;
        private readonly INotyfService _toastNotification;

        public DeleteModel(TwigaCRM.Data.ApplicationDbContext context, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _checkPermissions = checkPermissions;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public ProductCropAndAnimal ProductCropAndAnimal { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id, int cropandanimalId)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "remove_product");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            ProductCropAndAnimal = await _context.ProductCropAndAnimal
                .Include(p => p.CropAndAnimal)
                .Include(p => p.Product).FirstOrDefaultAsync(m => m.Id == id);

            if (ProductCropAndAnimal != null)
            {
                _context.ProductCropAndAnimal.Remove(ProductCropAndAnimal);
                //await _context.SaveChangesAsync();
                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                _toastNotification.Success("Product Removed!");
            }
            return RedirectToPage("../CropsAndAnimals/Details", new { id = cropandanimalId });
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductCropAndAnimal = await _context.ProductCropAndAnimal.FindAsync(id);

            if (ProductCropAndAnimal != null)
            {
                _context.ProductCropAndAnimal.Remove(ProductCropAndAnimal);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
