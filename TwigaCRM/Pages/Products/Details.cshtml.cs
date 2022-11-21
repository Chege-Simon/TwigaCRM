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

namespace TwigaCRM.Pages.Products
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

        public Product Product { get; set; }
        public List<CompetingProduct> CompetingProducts { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_product_details");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }
            CompetingProducts = await _context.CompetingProduct.ToListAsync();
            Product = await _context.Product
                .Include(p => p.BusinessLine)
                .Include(p => p.ProductCompetingProducts).FirstOrDefaultAsync(m => m.Id == id);

            ViewData["CompetingProducts"] = _context.CompetingProduct.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.ProductName + " - " + a.CompanyName
                                            }).ToList();
            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }
        [BindProperty]
        public ProductCompetingProduct ProductCompetingProduct { get; set; }
        public List<ProductCompetingProduct> ProductCompetingProducts { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_competing_product");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            ProductCompetingProducts = await _context.ProductCompetingProduct.ToListAsync();
            foreach (var productcompetingproduct in ProductCompetingProducts)
            {
                if (productcompetingproduct.CompetingProductId == ProductCompetingProduct.CompetingProductId && productcompetingproduct.ProductId == ProductCompetingProduct.ProductId)
                {
                    _toastNotification.Information("Competing Product Already Assigned!");
                    return RedirectToPage("./Details", new { id });
                }
            }
            _context.ProductCompetingProduct.Add(ProductCompetingProduct);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Competing Product Assigned!");

            return RedirectToPage("./Details", new { id });
        }
    }
}
