using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.Products
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly CheckPermissionsService _checkPermissions;
        private readonly INotyfService _toastNotification;
        private IHostEnvironment _environment;

        public IndexModel(TwigaCRM.Data.ApplicationDbContext context, INotyfService toastNotification, CheckPermissionsService checkPermissions, IHostEnvironment environment)
        {
            _context = context;
            _checkPermissions = checkPermissions;
            _toastNotification = toastNotification;
            _environment = environment;
        }

        public IList<Product> Products { get;set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_products");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            Products = await _context.Product
                .Include(p => p.BusinessLine).OrderByDescending(s => s.Id).ToListAsync();
            ViewData["BusinessLines"] = _context.BusinessLine.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.NormalizedName + " - " + a.Description
                                            }).ToList();
            return Page();
        }
        [BindProperty]
        public Product Product { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_product");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Products");
            }
            Products = await _context.Product
                .Include(p => p.BusinessLine).ToListAsync();
            foreach (var product in Products)
            {
                if (product.Code == Product.Code)
                {
                    _toastNotification.Warning("Product Already Exists, Check Code!");
                    ModelState.Clear();
                    return RedirectToPage("./Products");
                }
            }
            _context.Product.Add(Product);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Product Added!");

            return RedirectToPage("./Products");
        }
        [BindProperty]
        public IFormFile Upload { get; set; }
        public List<Product> NewProducts { get; set; }
        public async Task<IActionResult> OnPostImportAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "import_product");
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
            NewProducts = new List<Product>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(file, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        Product NewProduct = new Product();
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
                                int BusinessLineId = 0;
                                if (!string.IsNullOrWhiteSpace((string)reader.GetValue(6)))
                                {
                                    var BsusinessLineString = reader.GetValue(6).ToString();
                                    BusinessLine BusinessLine = await _context.BusinessLine.Where(t => t.Description.Contains(BsusinessLineString)).FirstOrDefaultAsync();
                                    if (BusinessLine == null)
                                    {

                                        _toastNotification.Error("Ensure All BusinessLines Are In the System!");

                                        return RedirectToPage("./Products");
                                    }
                                    BusinessLineId = BusinessLine.Id;
                                }
                                NewProduct = new()
                                {
                                    Name = reader.GetValue(0).ToString(),
                                    Description = reader.GetValue(1).ToString(),
                                    Code = reader.GetValue(2).ToString(),
                                    Price = (decimal)Convert.ToSingle(reader.GetValue(3)),
                                    UnitOfMeasure = reader.GetValue(4).ToString(),
                                    PackagingSize = (decimal)Convert.ToSingle(reader.GetValue(5)),
                                    //BusinessLineId = Convert.ToInt32(reader.GetValue(6))
                                    BusinessLineId = BusinessLineId
                                };
                            }

                            if (await _context.Product.Where(p => p.Code == NewProduct.Code).FirstOrDefaultAsync() == null)
                            {
                                _context.Product.Add(NewProduct);
                                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                            }
                        }
                    } while (reader.NextResult()); //Move to NEXT SHEET

                }
            }
            _toastNotification.Success("Products Added!");

            return RedirectToPage("./Products");
        }
    }
}
