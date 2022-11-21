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

namespace TwigaCRM.Pages.Calls
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public IndexModel(TwigaCRM.Data.ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        public IList<Call> Calls { get;set; }
        public AppUser AppUser { get; set; }
        public bool IsPermitted { get; private set; }
        public List<Permission> Permissions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_calls");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            Calls = await _context.Call
                .Include(c => c.CallType)
                .Include(c => c.Customer)
                .Include(c => c.Customer.Town)
                .Include(c => c.NonCustomerTown)
                .Include(c => c.SpokenTo).OrderByDescending(s => s.Id).ToListAsync();
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).ToListAsync();

            ViewData["Towns"] = _context.Town
                .Include(t => t.Region).Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name + " - " + a.Region.Name
                                            }).ToList();

            ViewData["Customers"] = _context.Customer
                .Include(t => t.Town)
                .Include(t => t.Town.Region).Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.ContactPersonName + " - " + a.SiteName + " @ " + a.Town.Name + " - " + a.Town.Region.Name
                                            }).ToList();
            ViewData["CallTypes"] = _context.CallType.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name + " - " + a.Description
                                            }).ToList();
            return Page();
        }
        [BindProperty]
        public Call Call { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_call");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("invalid Inputs!");
                return RedirectToPage("./Calls");
            }
            Call newCall = await _context.Call
              .Where(s => s.SpokenToId == Call.SpokenToId && s.CallType == Call.CallType && s.CallTime == Call.CallTime).FirstOrDefaultAsync();
            if(newCall != null)
            {
                _toastNotification.Error("Call Already Exist, Check Call Time!");
                return RedirectToPage("./Calls");
            }
            if (Call.ContectIsCustomer)
            {
                Call.NonCustomerTownId = null;
                Call.MobileNumber = null;
                Call.ContactCategory = null;
                Call.NonCustomerContactName = null;
                Customer Contect = await _context.Customer.FirstOrDefaultAsync(c => c.Id == Call.CustomerId);

                Call.ContactType = Contect.CustomerType == "Corporate" ? "Company" : "Individual";
            }
            Call.Status = "Open";
            _context.Call.Add(Call);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Call Created!");
            Call currentCall = await _context.Call
              .Where(s => s.SpokenToId == Call.SpokenToId && s.CallTypeId == Call.CallTypeId && s.CallTime == Call.CallTime).FirstOrDefaultAsync();

            return RedirectToPage("./Details", new { id = currentCall.Id });
        }
    }
}
