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
    public class IndexModel : PageModel
    {
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly CheckPermissionsService _checkPermissions;
        private readonly INotyfService _toastNotification;

        public IndexModel(TwigaCRM.Data.ApplicationDbContext context, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _checkPermissions = checkPermissions;
            _toastNotification = toastNotification;
        }

        public IList<CropAndAnimal> CropAndAnimals { get;set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_crops_and_animals");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            CropAndAnimals = await _context.CropAndAnimal
                .Include(c => c.Sector).OrderByDescending(s => s.Id).ToListAsync();
            ViewData["Sectors"] = _context.Sector.Select(a =>
                                           new SelectListItem
                                           {
                                               Value = a.Id.ToString(),
                                               Text = a.NormalizedName + " - " + a.Description
                                           }).ToList();
            return Page();
        }
        [BindProperty]
        public CropAndAnimal CropAndAnimal { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "add_crops_and_animals");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                //return Page();
                _toastNotification.Error("Invalid Inputs!");
                return RedirectToPage("./CropsAndAnimals");
            }

            _context.CropAndAnimal.Add(CropAndAnimal);
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success(CropAndAnimal.Type + " Added!");

            return RedirectToPage("./CropsAndAnimals");
        }
    }
}
