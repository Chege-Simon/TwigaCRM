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

namespace TwigaCRM.Pages.RAMCollectionTargets
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

        public RAMCollectionTarget RAMCollectionTarget { get; set; }

        public List<Permission> Permissions { get; set; }
        public IList<RAMCollectionTargetMapping> RAMCollectionTargetMappings { get; set; }
        [BindProperty]
        public int RAMCollectionTargetId { get; set; }
        public AppUser AppUser { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_RAM_Collection_target_details");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            RAMCollectionTargetId = (int)id;
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            RAMCollectionTarget = await _context.RAMCollectionTarget
                .Include(s => s.FinancialYear)
                .Include(s => s.RAM)
                .Include(s => s.RAM.UserSectors)
                .Include(s => s.RAM.UserBusinessLines)
                .Include(s => s.RAM.Town)
                .Include(u => u.RAM.Town.Region)
                .Include(s => s.RAM.AppRole).FirstOrDefaultAsync(m => m.Id == id);

            RAMCollectionTargetMappings = await _context.RAMCollectionTargetMapping
                .Include(t => t.MainDistributor)
                .Include(t => t.RAMCollectionTarget)
                .Where(t => t.RAMCollectionTargetId == RAMCollectionTarget.Id).ToListAsync();
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).ToListAsync();
            ApprovalStatus = RAMCollectionTarget.ApprovalStatus;
            if (RAMCollectionTarget == null)
            {
                return NotFound();
            }
            ViewData["MainDistributors"] =  _context.Customer.Include(c => c.Town).Include(c => c.Town.Region).Where(c => c.CustomerType == "Main Distributor" && c.Town.Region == AppUser.Town.Region).Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.ContactPersonName + " @ " + a.SiteName
                                            }).ToList();
            return Page();
        }
        [BindProperty]
        public RAMCollectionTargetMapping RAMCollectionTargetMapping { get; set; }
        [BindProperty]
        public string ApprovalStatus { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_RAM_Collection_target");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./Details", new { id = RAMCollectionTargetMapping.RAMCollectionTargetId });
            }
            if (RAMCollectionTarget.RAMId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../RAMCollectionTargets/RAMCollectionTargets");
            }
            _context.RAMCollectionTargetMapping.Add(RAMCollectionTargetMapping);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("RAMCollectionTargetMapping Recorded!");

            return RedirectToPage("./Details", new { id = RAMCollectionTargetMapping.RAMCollectionTargetId });
        }

        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "approve_RAM_collection_target");
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

            RAMCollectionTarget = await _context.RAMCollectionTarget.FirstOrDefaultAsync(d => d.Id == id);

            RAMCollectionTarget.IsSubmitted = ApprovalStatus != "Rejected" ? true : false;
            RAMCollectionTarget.ApprovalStatus = ApprovalStatus;
            _context.Attach(RAMCollectionTarget).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("R.A.M S.T Status Changed!");

            return RedirectToPage("./Details", new { id });
        }
        [BindProperty]
        public string Remarks { get; set; }
        public async Task<IActionResult> OnPostAddRemarksAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_RAM_Collection_target_remarks");
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

            RAMCollectionTarget = await _context.RAMCollectionTarget.FirstOrDefaultAsync(d => d.Id == id);
            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);

            RAMCollectionTarget.Remarks = RAMCollectionTarget.Remarks + System.Environment.NewLine + AppUser.FirstName + " " + AppUser.LastName + " ( " + AppUser.AppRole.Title + ") : " + Remarks;
            _context.Attach(RAMCollectionTarget).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Remarks Added!");

            return RedirectToPage("./Details", new { id });
        }
    }
}
