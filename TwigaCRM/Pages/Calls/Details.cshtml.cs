using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
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

namespace TwigaCRM.Pages.Calls
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

        public Call Call { get; set; }
        public List<Question> Questions { get; set; }
        public List<QuestionResponse> QuestionResponses { get; set; }
        public AppUser AppUser { get; set; }
        public List<Permission> Permissions { get; set; }
        public bool IsPermitted { get; private set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_call_details");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser = await _userManager.Users.Include(u => u.Town)
                .Include(u => u.Town.Region)
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            Permissions = await _context.Permission.Include(p => p.AppRolePermissions).ToListAsync();
            Call = await _context.Call
                .Include(c => c.CallType)
                .Include(c => c.Customer)
                .Include(c => c.NonCustomerTown)
                .Include(c => c.SpokenTo).FirstOrDefaultAsync(m => m.Id == id);
            Questions = await _context.Question.Include(q => q.Answers).Include(q => q.QuestionResponses).OrderBy(q => q.DisplayOrder).ToListAsync();
            QuestionResponses = await _context.Response.Where(r => r.CallId == Call.Id).ToListAsync();
            List<Product> AllProducts = await _context.Product.Include(p => p.BusinessLine).ToListAsync();
            List<Product> SelectableProducts = new List<Product>();
            //SelectableProducts.Add(AllProducts[0]);
            foreach (var product in AllProducts)
            {
                int index = SelectableProducts.FindIndex(item => item.Name == product.Name);
                if (index <= 0)
                {
                    SelectableProducts.Add(product);
                }
            }
            ViewData["Products"] = SelectableProducts.Select(a =>
                                            new SelectListItem
                                            {
                                                Value = a.Name.ToString(),
                                                Text = a.Name + " - " + a.BusinessLine.NormalizedName
                                            }).ToList();
            if (Call == null)
            {
                return NotFound();
            }
            return Page();
        }
        [BindProperty]
        public QuestionResponse QuestionResponse { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "view_call_details");
            if (!IsPermitted)
            {
                return RedirectToPage("/403");
            }
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);
            if (!ModelState.IsValid)
            {
                _toastNotification.Error("invalid Inputs!");
                return RedirectToPage("./Details", new { id });
            }
            Call = await _context.Call
                .Include(c => c.CallType)
                .Include(c => c.Customer)
                .Include(c => c.NonCustomerTown)
                .Include(c => c.SpokenTo).FirstOrDefaultAsync(m => m.Id == id);
            if (Call.Status == "Closed")
            {
                _toastNotification.Error("Call Closed, Changes Failed!");
                return RedirectToPage("./Details", new { id });
            }
            if (Call.SpokenToId != User?.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                _toastNotification.Warning("Failed, Contact Original Creator!");
                return RedirectToPage("../Calls/Calls");
            }
            QuestionResponse ExistingResponse = await _context.Response.Where(x => x.QuestionId == QuestionResponse.QuestionId && x.CallId == QuestionResponse.CallId).FirstOrDefaultAsync();
            if (ExistingResponse != null)
            {
                ExistingResponse.AnswerDesc = QuestionResponse.AnswerDesc;
                ExistingResponse.AnswerId = QuestionResponse.AnswerId;
                _context.Attach(ExistingResponse).State = EntityState.Modified;
            }
            else
            {
                _context.Response.Add(QuestionResponse);
            }
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Answer Saved!");

            return RedirectToPage("./Details", new { id });
        }
        [BindProperty]
        public string Status { get; set; }
        public async Task<IActionResult> OnPostCallStatusAsync(int id)
        {
            IsPermitted = _checkPermissions.CheckPermission(User, "change_call_status");
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
            Call = await _context.Call.FirstOrDefaultAsync(d => d.Id == id);

            Call.Status = Status;
            _context.Attach(Call).State = EntityState.Modified;
            //await _context.SaveChangesAsync();
            await _context.SaveChangesAsync(User?.FindFirst(ClaimTypes.NameIdentifier).Value);
            _toastNotification.Success("Call Status Changed!");

            return RedirectToPage("./Details", new { id });
        }
    }
}
