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

namespace TwigaCRM.Pages.Customers
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

        public Customer Customer { get; set; }
        public List<BusinessLine> BusinessLines { get; set; }
        public List<Sector> Sectors { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_customer_details");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            Sectors = await _context.Sector.Include(s => s.CustomerSectors).ToListAsync();
            BusinessLines = await _context.BusinessLine.Include(b => b.CustomerBusinessLines).ToListAsync();
            Customer = await _context.Customer
                .Include(c => c.Town)
                .Include(c => c.Zone)
                .Include(c => c.Town.Region)
                .Include(c => c.CustomerSectors)
                .Include(c => c.CustomerBusinessLines).FirstOrDefaultAsync(m => m.Id == id);

            if (Customer == null)
            {
                return NotFound();
            }

            ViewData["BusinessLines"] = _context.BusinessLine.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.NormalizedName + " - " + a.Description
                                            }).ToList();
            ViewData["Sectors"] = _context.Sector.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.NormalizedName + " - " + a.Description
                                            }).ToList();
            return Page();
        }
        [BindProperty]
        public CustomerBusinessLine CustomerBusinessLine { get; set; }
        public List<CustomerBusinessLine> CustomerBusinessLines { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAssignBusinessLineAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "assign_customer_businessline");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            CustomerBusinessLines = await _context.CustomerBusinessLine.ToListAsync();
            foreach (var customerbusinessline in CustomerBusinessLines)
            {
                if (customerbusinessline.BusinessLineId == CustomerBusinessLine.BusinessLineId && customerbusinessline.CustomerId == CustomerBusinessLine.CustomerId)
                {
                    _toastNotification.Information("Business Line Already Assigned!");
                    return RedirectToPage("./Details", new { id });
                }
            }
            _context.CustomerBusinessLine.Add(CustomerBusinessLine);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Business Line Assigned!");

            return RedirectToPage("./Details", new { id });
        }

        [BindProperty]
        public CustomerSector CustomerSector { get; set; }
        public List<CustomerSector> CustomerSectors { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAssignSectorAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "assign_customer_sector");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            CustomerSectors = await _context.CustomerSector.ToListAsync();
            foreach (var customersector in CustomerSectors)
            {
                if (customersector.SectorId == CustomerSector.SectorId && customersector.CustomerId == CustomerSector.CustomerId)
                {
                    _toastNotification.Information("Sector Already Assigned!");
                    return RedirectToPage("./Details", new { id });
                }
            }
            _context.CustomerSector.Add(CustomerSector);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Sector Assigned!");

            return RedirectToPage("./Details", new { id });
        }
    }
}
