using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TwigaCRM.Data;
using TwigaCRM.Models;

namespace TwigaCRM.Services
{
    public class CheckPermissionsService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;
        private bool isPermitted;
        public CheckPermissionsService(ApplicationDbContext context,
            UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }


        public bool CheckPermission( ClaimsPrincipal _user, string _perm)
        {
            ClaimsPrincipal User = _user;
            isPermitted = false;
            //var userId = System.Web.HttpContext.Current.User.Identity.Id
            var userId = _userManager.GetUserId(User);
            //var user = _context.Users.Include(x => x.AppRole.AppRole_Permissions).Where(u => u.Id == userId).ToList();
            var user = _context.Users.Include(x => x.AppRole)
                .Include(x => x.AppRole.AppRolePermissions).FirstOrDefault(u => u.Id == userId);
            List<AppRole_Permission> rolespermissions = user.AppRole.AppRolePermissions.ToList();

            foreach (var permission in rolespermissions)
            {
                var perm = _context.Permission.FirstOrDefault(p => p.Id == permission.PermissionId);
                if (perm.Name.Equals(_perm))
                {
                    isPermitted = true;
                }
            }
            if (!user.IsActivated)
            {
                isPermitted = false;
            }
            return isPermitted;
        }
        public List<string> NavPermissionAsync(ClaimsPrincipal _user)
        {
            ClaimsPrincipal User = _user;
            var userId = _userManager.GetUserId(User);
            var user = _userManager.Users
                .Include(u => u.AppRole)
                .Include(u => u.AppRole.AppRolePermissions).FirstOrDefault(u => u.Id == userId);
            var permissions = _context.Permission
                .Include(p => p.AppRolePermissions).ToList();
            List<string> shownav = new List<string>();
            var userpermissions = user.AppRole.AppRolePermissions;
            foreach (var apppermission in permissions)
            {
                foreach (var userpermission in userpermissions)
                {
                    if(apppermission.Id == userpermission.PermissionId)
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
                            shownav.Add("MTs");
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
                        case "generate_daily_movement_report":
                            shownav.Add("generate_daily_movement_report");
                            break;
                        case "generate_sales_target_report":
                            shownav.Add("generate_sales_target_report");
                            break;
                        case "view_competition_activities":
                            shownav.Add("competition_activities");
                            break;
                        case "generate_daily_collection_report":
                            shownav.Add("generate_daily_collection_report");
                            break;
                        case "generate_RAM_performance_reports":
                            shownav.Add("generate_RAM_performance_reports");
                            break;
                        case "generate_daily_sale_report":
                            shownav.Add("generate_daily_sale_report");
                            break;
                        case "view_daily_collections":
                            shownav.Add("daily_collections");
                            break;
                        case "view_daily_sales":
                            shownav.Add("daily_sales");
                            break;
                        case "view_DSRs":
                            shownav.Add("DSR");
                            break;
                        case "view_field_reports":
                            shownav.Add("field_reports");
                            break;
                        case "view_RAM_route_plans":
                            shownav.Add("RAM_route_plans");
                            break;
                        case "view_RAM_STs":
                            shownav.Add("RAM_STs");
                            break;
                        case "view_RAM_collection_targets":
                            shownav.Add("RAM_collection_targets");
                            break;
                        case "view_DCRs":
                            shownav.Add("DCR");
                            break;
                        case "generate_campaign_reports":
                            shownav.Add("generate_campaign_reports");
                            break;
                        case "generate_demo_reports":
                            shownav.Add("generate_demo_reports");
                            break;
                        case "generate_route_plan_reports":
                            shownav.Add("generate_route_plan_reports");
                            break;
                        case "view_pick_and_return_forms":
                            shownav.Add("view_pick_and_return_forms");
                            break;
                            default:
                        // code block
                        break;

                        }
                    }
                }

            }
            if (!user.IsActivated)
            {
                shownav.Clear();
            }
            return shownav;
            //return Task.FromResult<IEnumerable<string>>(
            //        shownav
            //    );
        }
    }
}
