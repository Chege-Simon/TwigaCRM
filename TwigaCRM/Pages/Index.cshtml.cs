using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;
using TwigaCRM.Models;
using TwigaCRM.Services;

namespace TwigaCRM.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private UserManager<AppUser> _userManager;
        private readonly TwigaCRM.Data.ApplicationDbContext _context;
        private readonly CheckPermissionsService _checkPermissions;

        public IndexModel(
            UserManager<AppUser> userManager, TwigaCRM.Data.ApplicationDbContext context, CheckPermissionsService checkPermissions)
        {
            _userManager = userManager;
            _context = context;
            _checkPermissions = checkPermissions;
        }
        public List<DailyMovementReport> DailyMovementReports { get; set; }
        public List<Call> Calls { get; set; }
        public List<AppUser> AppUsers { get; set; }
        public List<Customer> Customers { get; set; }
        public DailyMovementReport DailyMovementReport { get; set; }
        public List<Target> Targets { get; set; }
        public List<DailyMovement> DailyMovements { get; set; }
        public List<DailyMovementReport> DMRs { get; set; }
        public IList<SalesMovement> SalesMovements { get; set; }
        public List<RegionGraphModel> RegionGraphs { get; set; }
        public RegionGraphModel RegionGraph { get; set; }
        public class RegionGraphModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal ActualAmount { get; set; }
            public decimal TargetAmount { get; set; }
        }
        public async Task<IActionResult> OnGetAsync()
        {
            DailyMovementReports = await _context.DailyMovementReport.Include(d => d.DailyMovements).ToListAsync();
            Calls = await _context.Call.ToListAsync();
            AppUsers = await _userManager.Users.ToListAsync();
            Customers = await _context.Customer.ToListAsync();
            ViewData["shownav"] = _checkPermissions.NavPermissionAsync(User);


            DMRs = await _context.DailyMovementReport
                .Include(d => d.SalesPerson)
                .Include(d => d.SalesPerson.Town)
                .Include(d => d.SalesPerson.Town.Region)
                .Include(d => d.DailyMovements)
                .Where(d => d.SalesDate.Month == DateTime.Now.Month && d.TLstatus == "Approved").ToListAsync();

            FinancialYear CurrentFinancialYear = await _context.FinancialYear.Where(f => f.StartDate.Date <= DateTime.Now.Date && f.EndDate.Date >= DateTime.Now.Date).FirstOrDefaultAsync();
            DailyMovements = await _context.DailyMovement
                    .Include(d => d.Product)
                    .Include(d => d.DailyMovementReport)
                    .Include(d => d.DailyMovementReport.SalesPerson)
                    .Include(d => d.DailyMovementReport.SalesPerson.Town)
                    .Include(d => d.DailyMovementReport.SalesPerson.Town.Region)
                    .Where(d => d.DailyMovementReport.SalesDate.Month == DateTime.Now.Month && (d.DailyMovementReport.SalesDate.Date >= CurrentFinancialYear.StartDate.Date && d.DailyMovementReport.SalesDate.Date <= CurrentFinancialYear.EndDate.Date) && d.DailyMovementReport.TLstatus == "Approved").ToListAsync();
            SalesMovements = await _context.SalesMovement
                .Include(s => s.FinancialYear)
                .Include(s => s.SalesPerson)
                .Include(s => s.SalesPerson.Town)
                .Include(s => s.SalesPerson.Town.Region)
                .Include(s => s.Targets)
                .Where(s => s.TLstatus == "Approved").ToListAsync();

            Targets = await _context.Target
                    .Include(t => t.Product)
                    .Include(t => t.SalesMovement)
                    .Include(s => s.SalesMovement.SalesPerson)
                    .Include(s => s.SalesMovement.SalesPerson.Town)
                    .Include(s => s.SalesMovement.SalesPerson.Town.Region)
                    .Include(t => t.SalesMovement.FinancialYear)
                    .Where(d => d.SalesMovement.Month == DateTime.Now.Month && (d.SalesMovement.FinancialYear.StartDate.Year == DateTime.Now.Year || d.SalesMovement.FinancialYear.EndDate.Year == DateTime.Now.Year) && d.SalesMovement.TLstatus == "Approved").ToListAsync();
            List<Region> Regions = await _context.Region.ToListAsync();
            int i = 1;
            RegionGraphs = new List<RegionGraphModel>() { };
            foreach (var region in Regions)
            {
                decimal TargetQuantity = 0;
                decimal ActualQuantity = 0;
                decimal TargetValue = 0;
                decimal ActualValue = 0;
                foreach (var dm in DailyMovements)
                {
                    if (region == dm.DailyMovementReport.SalesPerson.Town.Region)
                    {
                        ActualQuantity = dm.Quantity;
                        ActualValue += (ActualQuantity / dm.Product.PackagingSize) * (dm.Product.Price * dm.Product.PackagingSize);
                    }
                }
                foreach (var target in Targets)
                {
                    if (region == target.SalesMovement.SalesPerson.Town.Region)
                    {
                        TargetQuantity = target.Volume;
                        TargetValue += (TargetQuantity / target.Product.PackagingSize) * (target.Product.Price * target.Product.PackagingSize);
                    }
                }


                RegionGraph = new RegionGraphModel { };
                RegionGraph.Id = i;
                RegionGraph.Name = region.Name;
                RegionGraph.ActualAmount = ActualValue;
                RegionGraph.TargetAmount = TargetValue;
                i++;
                RegionGraphs.Add(RegionGraph);
                RegionGraphs.OrderByDescending(r => r.TargetAmount);
            }
            return Page();
        }
    }
}