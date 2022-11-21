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

namespace TwigaCRM.Pages.Demos
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;
        private IHostEnvironment _environment;

        public DetailsModel(TwigaCRM.Data.ApplicationDbContext context, IHostEnvironment environment, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
            _environment = environment;
        }

        public AppUser AppUser { get; set; }
        public List<Permission> Permissions { get; set; }
        public IList<DemoDetail> DemoDetails { get; set; }
        public IList<TwigaCRM.Models.DemoProductRequisition> DemoProductRequisitions { get; set; }
        public Demo Demo { get; set; }
        [BindProperty]
        public string FOAstatus { get; set; }
        [BindProperty]
        public string PDstatus { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_demo_details");
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
            Demo = await _context.Demo
                .Include(c => c.SalesPerson)
                .Include(c => c.SalesPerson.Town)
                .Include(u => u.SalesPerson.AppRole)
                .Include(c => c.SalesPerson.UserBusinessLines)
                .Include(c => c.SalesPerson.UserSectors).FirstOrDefaultAsync(u => u.Id == id);
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            FOAstatus = Demo.FOAstatus;
            PDstatus = Demo.PDstatus;
            Status = Demo.Status;
            DemoDetails = await _context.DemoDetail
                .Include(r => r.Demo)
                .Include(r => r.Product)
                .Include(r => r.DemoProgressReports)
                .Include(r => r.Site)
                .Include(r => r.Site.Town)
                .Include(r => r.Site.Zone)
                .Include(r => r.Site.Town.Region)
                .Include(r => r.PestAndDisease)
                .Include(r => r.CropAndAnimal)
                .Include(r => r.CompetingProduct).Where(d => d.Demo.Id == Demo.Id).OrderByDescending(s => s.Id).ToListAsync();
            DemoProductRequisitions = await _context.DemoProductRequisition
                .Include(r => r.Demo)
                .Include(r => r.Product)
                .Include(r => r.Stockist).Where(d => d.Demo.Id == Demo.Id).OrderByDescending(s => s.Id).ToListAsync();
            List<Product> AllProducts = _context.Product
                .Include(t => t.BusinessLine)
                .Include(t => t.ProductsCropsAndAnimals).ToList();

            List<CropAndAnimal> SelectableCropAndAnimals = new() { };
            List<Product> SelectableProducts = new() { };
            List<CompetingProduct> SelectableCompetingProducts = new() { };
            List<PestAndDisease> SelectablePestAndDiseases = new() { };
            foreach (var CropAndAnimal in _context.CropAndAnimal.Include(c => c.Sector).Include(c => c.ProductsCropsAndAnimals).Include(c => c.CropsAndAnimalsPestsAndDiseases).ToList())
            {
                foreach (var sector in Demo.SalesPerson.UserSectors)
                {
                    if (CropAndAnimal.SectorId == sector.SectorId)
                    {
                        SelectableCropAndAnimals.Add(CropAndAnimal);
                    }
                }
                foreach (var product in CropAndAnimal.ProductsCropsAndAnimals)
                {
                    Product temp = _context.Product.Include(p => p.ProductCompetingProducts).FirstOrDefault(p => p.Id == product.ProductId);
                    if (!SelectableProducts.Contains(temp))
                    {
                        SelectableProducts.Add(temp);
                    }
                }

            }
            foreach (var product in SelectableProducts)
            {
                foreach (var competingproduct in product.ProductCompetingProducts)
                {
                    CompetingProduct temp = _context.CompetingProduct.FirstOrDefault(p => p.Id == competingproduct.CompetingProductId);
                    if (!SelectableCompetingProducts.Contains(temp))
                    {
                        SelectableCompetingProducts.Add(temp);
                    }
                }
            }
            foreach (var cropandanimal in SelectableCropAndAnimals)
            {
                foreach (var pestanddisease in cropandanimal.CropsAndAnimalsPestsAndDiseases)
                {
                    PestAndDisease temp = _context.PestAndDisease.FirstOrDefault(p => p.Id == pestanddisease.PestAndDiseaseId);
                    if (!SelectablePestAndDiseases.Contains(temp))
                    {
                        SelectablePestAndDiseases.Add(temp);
                    }
                }
            }
            ViewData["CropAndAnimals"] = SelectableCropAndAnimals.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name
                                            }).ToList();

            ViewData["Products"] = SelectableProducts.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name + " (" + a.Description + ") - " + " (" + a.Code + ") - " + a.BusinessLine.NormalizedName
                                            }).ToList();
            ViewData["CompetingProducts"] = SelectableCompetingProducts.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.ProductName + " - " + a.CompanyName
                                            }).ToList();
            ViewData["PestAndDiseases"] = SelectablePestAndDiseases.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name + " - " + a.Description
                                            }).ToList();
            ViewData["Sites"] = _context.Customer.Include(c => c.Town).Where(c => c.CustomerType == "Farmer").Where(c => c.Town.Id == Demo.SalesPerson.Town.Id).Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.SiteName + " - " + a.FarmerType
                                            }).ToList();
            ViewData["DemoProducts"] = _context.DemoDetail.Include(c => c.Product).Where(c => c.DemoId == Demo.Id).Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Product.Id.ToString(),
                                                Text = a.Product.Description
                                            }).ToList();
            ViewData["Stockists"] = _context.Customer.Include(c => c.Town).Where(c => c.CustomerType == "Stockist").Where(c => c.Town.Id == Demo.SalesPerson.Town.Id).Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.SiteName
                                            }).ToList();

            if (Demo == null)
            {
                return NotFound();
            }
            return Page();
        }
        [BindProperty]
        public DemoDetail DemoDetail { get; set; }
        public DemoDetail RecordedDemoDetail { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAddDetailsAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_demo_detail");
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
            RecordedDemoDetail = await _context.DemoDetail.Where(c => c.DemoId == id && c.ProductId == DemoDetail.ProductId && c.SiteId == DemoDetail.SiteId).FirstOrDefaultAsync();
            if (RecordedDemoDetail != null)
            {
                _toastNotification.Information("Product And Site Pair Already Added!");
                return RedirectToPage("./Details", new { id });
            }

            Demo Demo = await _context.Demo
                .Include(c => c.SalesPerson).FirstOrDefaultAsync(m => m.Id == DemoDetail.DemoId);
            if (Demo.SalesPersonId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../Demos/Demos");
            }
            Customer CurrentFarmer = await _context.Customer.FirstOrDefaultAsync(c => c.Id == DemoDetail.SiteId);
            if (DemoDetail.RequestedQuantityOfSample <= 0)
            {
                decimal testArea = 0.0015M;
                DemoDetail.RequestedQuantityOfSample = testArea;
            }
            Product CurrentProduct = await _context.Product.FirstOrDefaultAsync(p => p.Id == DemoDetail.ProductId);
            ProductCropAndAnimal CurrentCropAndAnimal = await _context.ProductCropAndAnimal.Where(c => c.CropAndAnimalId == DemoDetail.CropAndAnimalId && c.ProductId == DemoDetail.ProductId).FirstOrDefaultAsync();
            DemoDetail.RequestedVolumeOfProduct = ((DemoDetail.RequestedQuantityOfSample * DemoDetail.RequestedNumberOfDemos) / CurrentCropAndAnimal.Servings) * CurrentProduct.PackagingSize;
            _context.DemoDetail.Add(DemoDetail);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Details Added!");

            return RedirectToPage("./Details", new { id = DemoDetail.DemoId });

        } 
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD




        public async Task<IActionResult> OnPostFOAApproveAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "foa_approve_demo");
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
            Demo = await _context.Demo.FirstOrDefaultAsync(d => d.Id == id);
            List<DemoDetail> reqDetails = await _context.DemoDetail
                .Include(r => r.Demo)
                .Include(r => r.Product)
                .Where(r => r.DemoId == Demo.Id).ToListAsync();
            foreach (var item in reqDetails)
            {
                if (!item.IsFOAApproved)
                {
                    _toastNotification.Information("Product: " + item.Product.Name + "(" + item.Product.Code + ")" + " NOT APPROVED!");
                    return RedirectToPage("./Details", new { id });
                }
            }
            Demo.IsSubmitted = FOAstatus != "Rejected" ? true : false;
            Demo.PDstatus = Demo.FOAstatus != FOAstatus ? "Pending" : Demo.PDstatus;
            Demo.FOAstatus = FOAstatus;
            _context.Attach(Demo).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Demo Approval Changed!");

            return RedirectToPage("./Details", new { id });
        }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostPDApproveAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "pd_approve_demo");
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

            Demo = await _context.Demo.FirstOrDefaultAsync(d => d.Id == id);

            Demo.IsSubmitted = PDstatus != "Rejected" ? true : false;
            Demo.PDstatus = PDstatus;
            _context.Attach(Demo).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Demo Approval Changed!");

            return RedirectToPage("./Details", new { id });
        }
        [BindProperty]
        public string Remarks { get; set; }
        public async Task<IActionResult> OnPostAddRemarksAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_demo_remarks");
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

            Demo = await _context.Demo.FirstOrDefaultAsync(d => d.Id == id);
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);

            Demo.Remarks = Demo.Remarks + System.Environment.NewLine + AppUser.FirstName + " " + AppUser.LastName + " ( " + AppUser.AppRole.Title + ") : " + Remarks;
            _context.Attach(Demo).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Remarks Added!");

            return RedirectToPage("./Details", new { id });
        }
        [BindProperty]
        public string Status { get; set; }
        public async Task<IActionResult> OnPostDemoStatusAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "change_demo_status");
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

            Demo = await _context.Demo.FirstOrDefaultAsync(d => d.Id == id);

            Demo.Status = Status;
            _context.Attach(Demo).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Demo Status Changed!");

            return RedirectToPage("./Details", new { id });
        }
        [BindProperty]
        public TwigaCRM.Models.DemoProductRequisition DemoProductRequisition { get; set; }
        [BindProperty]
        public IFormFile Upload { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostProductRequisitionAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_demo_requisition");
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
            var fileName = DateTime.Now.Ticks + Upload.FileName;
            System.IO.Directory.CreateDirectory("uploads");
            var file = Path.Combine(_environment.ContentRootPath, "uploads", fileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }
            DemoProductRequisition.InvoiceUrl = fileName;
            _context.DemoProductRequisition.Add(DemoProductRequisition);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Product Requisition Added!");

            return RedirectToPage("./Details", new { id = DemoProductRequisition.DemoId });

        }
        public FileResult OnGetDownloadFile(string fileName)
        {
            //Build the File Path.
            string path = Path.Combine(_environment.ContentRootPath, "uploads/") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            _toastNotification.Success("Invoice Downloaded!");
            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }
    }
}
