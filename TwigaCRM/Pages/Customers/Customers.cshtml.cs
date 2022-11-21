using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Claims;
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

namespace TwigaCRM.Pages.Customers
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

        public IList<Customer> Customers { get;set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_customers");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            Customers = await _context.Customer
                .Include(c => c.Town).OrderByDescending(s => s.Id).ToListAsync();
            ViewData["Towns"] = _context.Town.Include(t => t.Region).Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name + " - " + a.Region.Name
                                            }).ToList();
            ViewData["Zones"] = _context.Zone.Include(t => t.Town).Include(t => t.Town.Region).Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name + " - " + a.Town.Name + " - " + a.Town.Region.Name
                                            }).ToList();
            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "create_customers");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Customers");
            }
            Customers = await _context.Customer.ToListAsync();
            foreach (var customer in Customers)
            {
                if (customer.ContactPersonName == Customer.ContactPersonName && customer.PhoneNumber == Customer.PhoneNumber)
                {
                    _toastNotification.Warning("Customer Already Exists!");
                    return RedirectToPage("./Customers");
                }
            }
            if(Customer.CustomerType != "Farmer")
            {
                Customer.FarmerType = null;
            }
            _context.Customer.Add(Customer);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Customer-"+Customer.CustomerType+" Added!");

            return RedirectToPage("./Customers");
        }
        [BindProperty]
        public IFormFile Upload { get; set; }
        public List<Customer> NewCustomers { get; set; }
        public async Task<IActionResult> OnPostImportAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "import_customers");
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
            NewCustomers = new List<Customer>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(file, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        Customer NewCustomer = new Customer();
                        while (reader.Read()) //Each ROW
                        {
                            for (int column = 0; column < reader.FieldCount; column++)
                            {
                                //Console.WriteLine(reader.GetString(column));//Will blow up if the value is decimal etc. 
                                //Console.WriteLine(reader.GetValue(column));//Get Value returns object
                                int? ZoneId = 0;
                                int TownId = 0;
                                string FarmerType = "";
                                string FarmSize = "";
                                if (!reader.IsDBNull(5))
                                {
                                    var TownName = reader.GetValue(5).ToString();
                                    Town Town = await _context.Town.Where(t => t.Name == TownName).FirstOrDefaultAsync();
                                    if (Town == null)
                                    {

                                        _toastNotification.Error("Ensure " + TownName + " Is In the System!");

                                        return RedirectToPage("./Customers");
                                    }
                                    TownId = Town.Id;
                                }
                                if (reader.FieldCount >= 7)
                                {
                                    //if (!string.IsNullOrWhiteSpace((string)reader.GetValue(6)))
                                    if (!reader.IsDBNull(6))
                                    {
                                        var ZoneName = reader.GetValue(6).ToString();
                                        Zone Zone = await _context.Zone.Where(t => t.Name == ZoneName).FirstOrDefaultAsync();

                                        if (Zone == null)
                                        {
                                            ZoneId = 0;
                                            _toastNotification.Error("Ensure " + ZoneName + " Is In the System!");
                                            return RedirectToPage("./Customers");
                                        }
                                        else
                                        {
                                            ZoneId = Zone.Id;
                                        }
                                    }
                                }
                                var customertype = reader.GetString(4).ToString();
                                if (customertype != "Farmer")
                                {
                                    FarmerType = "";
                                    FarmSize = "";
                                }
                                else
                                {
                                    FarmerType = reader.GetString(7).ToString();
                                    FarmSize = reader.GetString(8).ToString();
                                }
                                NewCustomer = new()
                                {
                                    ContactPersonName = reader.GetValue(0).ToString(),
                                    SiteName = reader.GetValue(1).ToString(),
                                    PhoneNumber = reader.GetValue(2).ToString(),
                                    Email = reader.GetValue(3).ToString(),
                                    CustomerType = reader.GetValue(4).ToString(),
                                    TownId = TownId,
                                    ZoneId = ZoneId == 0 ? null : ZoneId,
                                    FarmerType = FarmerType,
                                    FarmSize = FarmSize
                                };
                            }
                            if (await _context.Customer.Where(p => p.Email == NewCustomer.Email).FirstOrDefaultAsync() == null)
                            {
                                _context.Customer.Add(NewCustomer);
                                await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
                            }
                        }
                    } while (reader.NextResult()); //Move to NEXT SHEET

                }
            }
            _toastNotification.Success("Customers Added!");

            return RedirectToPage("./Customers");
        }
    }
}
