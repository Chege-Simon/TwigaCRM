using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwigaCRM.Data;
using TwigaCRM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TwigaCRM.Pages.Shared.AdminLTE
{
    [Authorize]
    public class _LayoutModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;
        public _LayoutModel(TwigaCRM.Data.ApplicationDbContext context,
            UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            //var user = _context.Users.Include(x => x.AppRole.AppRole_Permissions).Where(u => u.Id == userId).ToList();
            //var user = _context.Users
            //    .Include(x => x.AppRole)
            //    .Include(x => x.AppRole.AppRolePermissions).FirstOrDefault(u => u.Id == userId);

            var Id = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.Users
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefaultAsync(u => u.Id == Id);
            var permissions = await _context.Permission
                .Include(p => p.AppRolePermissions).ToListAsync();
            ViewData["AppUser"] = user;
            ViewData["AppPermissions"] = permissions;
            List<string> shownav = new List<string>();
            var userpermissions = user.AppRole.AppRolePermissions;
            foreach (var apppermission in permissions)
            {
                foreach (var userpermission in userpermissions)
                {
                    switch (apppermission.Name)
                    {
                        case "view_roles":
                            shownav.Add("roles");
                            break;
                        case "view_users":
                            shownav.Add("users");
                            break;
                        case "view_regions":
                            shownav.Add("regions");
                            break;
                        case "view_customers":
                            shownav.Add("customers");
                            break;
                        case "view_products":
                            shownav.Add("products");
                            break;
                        case "view_crops_and_animals":
                            shownav.Add("crops_and_animals");
                            break;
                        case "view_competing_products":
                            shownav.Add("competing_products");
                            break;
                        case "view_campaign_items":
                            shownav.Add("campaign_items");
                            break;
                        case "view_daily_movements":
                            shownav.Add("daily_movements");
                            break;
                        case "view_MTs":
                            shownav.Add("mts");
                            break;
                        case "view_stock_takes":
                            shownav.Add("stock_takes");
                            break;
                        case "view_route_plans":
                            shownav.Add("route_plans");
                            break;
                        case "view_campaigns":
                            shownav.Add("campaigns");
                            break;
                        case "view_demos":
                            shownav.Add("demos");
                            break;
                        case "view_campaign_budgets":
                            shownav.Add("campaign_budgets");
                            break;
                        case "view_expense_categories":
                            shownav.Add("expense_categories");
                            break;
                        case "view_calls":
                            shownav.Add("calls");
                            break;
                        case "view_calltypes":
                            shownav.Add("calltypes");
                            break;
                        case "view_questions":
                            shownav.Add("questions");
                            break;
                        case "view_question_responses":
                            shownav.Add("question_responses");
                            break;
                        case "view_DMRs":
                            shownav.Add("DMRs");
                            break;
                        case "generate_performance_reports":
                            shownav.Add("generate_performance_reports");
                            break;
                        case "view_audit_trail":
                            shownav.Add("audit_trail");
                            break;
                        case "view_businesslines_and_sectors":
                            shownav.Add("businesslines_and_sectors");
                            break;
                        case "view_pests_and_diseases":
                            shownav.Add("pests_and_diseases");
                            break;
                        case "view_permissions":
                            shownav.Add("permissions");
                            break;
                        case "view_financial_years":
                            shownav.Add("financial_years");
                            break;
                        default:
                            // code block
                            break;
                    }
                }

            }
            ViewData["shownav"] = shownav;
        }
    }
}
