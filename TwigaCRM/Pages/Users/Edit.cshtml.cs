using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using TwigaCRM.Data;
using TwigaCRM.Models;
using TwigaCRM.Areas.Identity.Pages.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TwigaCRM.Services;

namespace TwigaCRM.Pages.Users
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly CheckPermissionsService _checkPermissions;

        public EditModel(ApplicationDbContext context,
            UserManager<AppUser> userManager,
            ILogger<RegisterModel> logger,
            SignInManager<AppUser> signInManager,
            INotyfService notyf, CheckPermissionsService checkPermissions)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _notyf = notyf;
            _signInManager = signInManager;
            _checkPermissions = checkPermissions;
        }
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        [BindProperty]
        public InputModel Input { get; set; }
        public AppUser AppUser { get; set; }


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
        }
        public string ChangePasswordUrl { get; set; }
        public bool IsPermitted { get; private set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_user");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            var editUser = await _userManager.FindByIdAsync(id);
            AppUser = editUser;
            ViewData["user"] = editUser;
            ViewData["AppRoles"] = _context.AppRole.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Title + " - " + a.Description
                                            }).ToList();
            ViewData["Towns"] = _context.Town.Include(t => t.Region).Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Id.ToString(),
                                                Text = a.Name + " - " + a.Region.Name
                                            }).ToList();

            Input = new InputModel
            {
                AppRoleId = editUser.UserAppRoleId.ToString(),
                Email = editUser.Email,
                FirstName = editUser.FirstName,
                LastName = editUser.LastName,
                PhoneNumber = editUser.PhoneNumber,
                Status = editUser.IsActivated,
                TownId = editUser.TownId.ToString()
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_user");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }

            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            //if (!ModelState.IsValid)
            //{
            //    return RedirectToPage("./Users");
            //}
            var userEdited = await _userManager.FindByIdAsync(id);

            //if (!ModelState.IsValid)
            //{
            //    await LoadAsync(user);
            //    return Page();
            //}

            var phoneNumber = await _userManager.GetPhoneNumberAsync(userEdited);
            if (Input.PhoneNumber != phoneNumber)
            {
                await _userManager.SetPhoneNumberAsync(userEdited, Input.PhoneNumber);
            }
            var firstName = userEdited.FirstName;
            if (Input.FirstName != firstName)
            {
                userEdited.FirstName = Input.FirstName;
            }
            var email = userEdited.Email;
            if (Input.Email != email)
            {
                userEdited.Email = Input.Email;
                userEdited.UserName = Input.Email;
            }

            var lastName = userEdited.LastName;
            if (Input.LastName != lastName)
            {
                userEdited.LastName = Input.LastName;
            }
            var status = userEdited.IsActivated;
            if (Input.Status != status)
            {
                userEdited.IsActivated = Input.Status;
            }
            var AppRoleId = userEdited.UserAppRoleId;
            if (Int32.Parse(Input.AppRoleId) != AppRoleId)
            {
                userEdited.UserAppRoleId = Int32.Parse(Input.AppRoleId);
            }
            var TownId = userEdited.TownId;
            if (Input.TownId != null && Int32.Parse(Input.TownId) != TownId)
            {
                userEdited.TownId = Int32.Parse(Input.TownId);
            }
            if (Input.TownId == null)
            {
                userEdited.TownId = null;
            }

            var result = await _userManager.UpdateAsync(userEdited);
            if (!result.Succeeded)
            {

                foreach (var error in result.Errors)
                {
                    //ModelState.AddModelError(string.Empty, );
                    _notyf.Error(error.Code);
                }
                return RedirectToPage("./Users");
            }
            _notyf.Success("User Edited Successfully!");

            if (userEdited == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return RedirectToPage("./Edit", new { id});
        }
        public class PasswordModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
        [BindProperty]
        public PasswordModel PasswordInput { get; set; }
        public async Task<IActionResult> OnPostNewPasswordAsync(string id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "edit_user");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);

            var userEdited = await _userManager.FindByIdAsync(id);
            var token = await _userManager.GeneratePasswordResetTokenAsync(userEdited);

            var changePasswordResult = await _userManager.ResetPasswordAsync(userEdited, token, PasswordInput.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            else
            {
                await _signInManager.RefreshSignInAsync(userEdited);
                _notyf.Success("User Password Edited Successfully!");
            }
            return RedirectToPage("./Edit", new { id });
        }
    }
}
