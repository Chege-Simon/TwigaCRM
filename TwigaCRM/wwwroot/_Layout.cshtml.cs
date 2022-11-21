using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using J.E.Data;
using J.E.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace J.E.Pages.Shared.AdminLTE
{
    public class _LayoutModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public _LayoutModel(J.E.Data.ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public List<AppRole_Permission> CurrentUserPermissions { get; set; }
        public void OnGet()
        {
            var userId = _userManager.GetUserId(User);
            //var user = _context.Users.Include(x => x.AppRole.AppRole_Permissions).Where(u => u.Id == userId).ToList();
            var user = _context.Users.Include(x => x.AppRole.AppRole_Permissions).FirstOrDefault(u => u.Id == userId);
            CurrentUserPermissions = user.AppRole.AppRole_Permissions;
            ViewData["CurrentUserPermissions"] = CurrentUserPermissions;
            ViewData["hello"] = "world";
        }
    }
}
