using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TwigaCRM.Data;
using TwigaCRM.Models;
using NToastNotify;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.Users
{
    [Authorize]
    public class UsersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly INotyfService _toastNotification;
        private readonly CheckPermissionsService _checkPermissions;

        public AppUser NewUser { get; set; }

        public UsersModel(ApplicationDbContext context, UserManager<AppUser> userManager, INotyfService toastNotification, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _toastNotification = toastNotification;
            _checkPermissions = checkPermissions;
        }

        public List<AppUser> Users { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "FirstName")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "LastName")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "PhoneNumber")]
            [DataType(DataType.PhoneNumber)]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "AppRoleId")]
            public string AppRoleId { get; set; }

            
            [Display(Name = "TownId")]
            public string TownId { get; set; }


            [Required]
            [Display(Name = "Status")]
            public bool Status { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_users");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            Users = await _userManager.Users.Include(u => u.AppRole)
                .Include(x => x.AppRole.AppRolePermissions).ToListAsync();
            ViewData["AppRoles"] = _context.AppRole.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Title
                                            }).ToList();
            ViewData["Towns"] = _context.Town.Include(t => t.Region).Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name + " - " + a.Region.Name
                                            }).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "create_users");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("invalid Inputs!");
                return RedirectToPage("./Users");
            }
            var role = Input.AppRoleId;
            var user = new AppUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                UserAppRoleId = Int32.Parse(role),
                CreatedAt = DateTime.Now,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                PhoneNumber = Input.PhoneNumber,
                TownId = Int32.Parse(Input.TownId),
                IsActivated = Input.Status,
            };
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    _toastNotification.Error(error.Code);
                }
                return RedirectToPage("./Users");
            }
            _toastNotification.Success("User Created Successfully!");

            return RedirectToPage("./Users");
        }
    }
}
