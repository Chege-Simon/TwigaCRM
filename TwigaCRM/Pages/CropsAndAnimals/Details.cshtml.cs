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

namespace TwigaCRM.Pages.CropsAndAnimals
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly CheckPermissionsService _checkPermissions;
        private readonly INotyfService _toastNotification;

        public DetailsModel(TwigaCRM.Data.ApplicationDbContext context,
            INotyfService toastNotification,
            CheckPermissionsService checkPermissions)
        {
            _context = context;
            _checkPermissions = checkPermissions;
            _toastNotification = toastNotification;
        }

        public CropAndAnimal CropAndAnimal { get; set; }
        public List<PestAndDisease> PestsAndDiseases { get; set; }
        public List<Product> Products { get; set; }
        public bool IsPermitted { get; private set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_crops_and_animals_details");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }
            PestsAndDiseases = await _context.PestAndDisease
                .Include(c => c.CropsAndAnimalsPestsAndDiseases).ToListAsync();
            Products = await _context.Product
                .Include(c => c.BusinessLine)
                .Include(c => c.ProductsCropsAndAnimals).ToListAsync();
            CropAndAnimal = await _context.CropAndAnimal
                .Include(c => c.CropsAndAnimalsPestsAndDiseases)
                .Include(c => c.ProductsCropsAndAnimals)
                .Include(c => c.Sector).FirstOrDefaultAsync(m => m.Id == id);

            if (CropAndAnimal == null)
            {
                return NotFound();
            }
            ViewData["PestsAndDiseases"] = _context.PestAndDisease.Select(a =>
                                           new SelectListItem
                                           {
                                               Value = a.Id.ToString(),
                                               Text = a.Name + " - " + a.Description
                                           }).ToList();
            ViewData["Products"] = _context.Product.Select(a =>
                                           new SelectListItem
                                           {
                                               Value = a.Id.ToString(),
                                               Text = a.Name + " (" + a.Description + ") - " + " (" + a.Code + ") - " + a.BusinessLine.NormalizedName
                                           }).ToList();
            return Page();
        }

        [BindProperty]
        public CropAndAnimalPestAndDisease CropAndAnimalPestAndDisease { get; set; }
        public List<CropAndAnimalPestAndDisease> CropsAndAnimalsPestsAndDiseases { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAssignPestAndDiseaseAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "assign_pests_and_diseases");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            CropsAndAnimalsPestsAndDiseases = await _context.CropAndAnimalPestAndDisease.ToListAsync();
            foreach (var cropandanimalpestanddisease in CropsAndAnimalsPestsAndDiseases)
            {
                if (cropandanimalpestanddisease.CropAndAnimalId == CropAndAnimalPestAndDisease.CropAndAnimalId && cropandanimalpestanddisease.PestAndDiseaseId == CropAndAnimalPestAndDisease.PestAndDiseaseId)
                {
                    _toastNotification.Information("Pest / Disease Already Assigned!");
                    return RedirectToPage("./Details", new { id });
                }
            }

            _context.CropAndAnimalPestAndDisease.Add(CropAndAnimalPestAndDisease);
            //await _context.SaveChangesAsync();

            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Pest / Disease Assigned!");

            return RedirectToPage("./Details", new { id });
        }
        [BindProperty]
        public ProductCropAndAnimal ProductCropAndAnimal { get; set; }
        public List<ProductCropAndAnimal> ProductsCropsAndAnimals { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAssignProductAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "assign_product");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            ProductsCropsAndAnimals = await _context.ProductCropAndAnimal.ToListAsync();
            foreach (var productcropandanimal in ProductsCropsAndAnimals)
            {
                if (productcropandanimal.CropAndAnimalId == ProductCropAndAnimal.CropAndAnimalId && productcropandanimal.ProductId == ProductCropAndAnimal.ProductId)
                {
                    _toastNotification.Information("Product Already Assigned!");
                    return RedirectToPage("./Details", new { id });
                }
            }

            _context.ProductCropAndAnimal.Add(ProductCropAndAnimal);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Product Assigned!");

            return RedirectToPage("./Details", new { id });
        }
    }
}
