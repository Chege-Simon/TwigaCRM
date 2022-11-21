using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TwigaCRM.Models;
using TwigaCRM.Services;
using Newtonsoft.Json.Linq;

namespace TwigaCRM.Pages.Charts
{
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
        public List<ProductGraphModel> ProductsGraphs { get; set; }
        public ProductGraphModel ProductGraph { get; set; }
        public DailyMovementReport DailyMovementReport { get; set; }
        public List<Target> Targets { get; set; }
        public List<DailyMovement> DailyMovements { get; set; }
        public List<DailyMovementReport> DailyMovementReports { get; set; }
        public IList<SalesMovement> SalesMovements { get; set; }
        public class ProductGraphModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal ActualAmount { get; set; }
            public decimal TargetAmount { get; set; }
        }
        public async Task<JsonResult> OnGetAsync()
        {
            DailyMovementReports = await _context.DailyMovementReport
                .Include(d => d.SalesPerson)
                .Include(d => d.DailyMovements)
                .Where(d => d.SalesDate.Month == DateTime.Now.Month && d.TLstatus == "Approved").ToListAsync();

            FinancialYear CurrentFinancialYear = await _context.FinancialYear.Where(f => f.StartDate.Date <= DateTime.Now.Date && f.EndDate.Date >= DateTime.Now.Date).FirstOrDefaultAsync();
            DailyMovements = await _context.DailyMovement
                    .Include(d => d.Product)
                    .Include(d => d.DailyMovementReport)
                    .Where(d => d.DailyMovementReport.SalesDate.Month == DateTime.Now.Month && (d.DailyMovementReport.SalesDate.Date >= CurrentFinancialYear.StartDate.Date && d.DailyMovementReport.SalesDate.Date <= CurrentFinancialYear.EndDate.Date) && d.DailyMovementReport.TLstatus == "Approved").ToListAsync();
            SalesMovements = await _context.SalesMovement
                .Include(s => s.FinancialYear)
                .Include(s => s.SalesPerson)
                .Include(s => s.Targets)
                .Where(s => s.TLstatus == "Approved").ToListAsync();

            Targets = await _context.Target
                    .Include(t => t.Product)
                    .Include(t => t.SalesMovement)
                    .Include(t => t.SalesMovement.FinancialYear)
                    .Where(d => d.SalesMovement.Month == DateTime.Now.Month && d.SalesMovement.FinancialYear.Id == CurrentFinancialYear.Id && d.SalesMovement.TLstatus == "Approved").ToListAsync();

            int i = 1;
            ProductsGraphs = new List<ProductGraphModel>() { };
            //foreach (var target in Targets)
            //{
            //    decimal TargetQuantity = 0;
            //    TargetQuantity = target.Volume;
            //    decimal ActualQuantity = 0;
            //    foreach (var dm in DailyMovements)
            //    {
            //        if (target.Product == dm.Product)
            //        {
            //            ActualQuantity += dm.Quantity;
            //        }
            //    }

            //    decimal TargetValue = (TargetQuantity / target.Product.PackagingSize) * (target.Product.Price * target.Product.PackagingSize);
            //    decimal ActualValue = (ActualQuantity / target.Product.PackagingSize) * (target.Product.Price * target.Product.PackagingSize);
            //    ProductGraph = new ProductGraphModel { };
            //    ProductGraph.Id = i;
            //    ProductGraph.Name = target.Product.Name;
            //    ProductGraph.ActualAmount = ActualValue;
            //    ProductGraph.TargetAmount = TargetValue;
            //    i++;
            //    ProductsGraphs.Add(ProductGraph);
            //}
            foreach (var product in _context.Product.GroupBy(p => p.Name).Select(g => new { name = g.Key, packsize = g.Count() }))
            {
                decimal ActualValue = 0;
                decimal TargetValue = 0;
                foreach (var target in Targets)
                {
                    if (product.name == target.Product.Name)
                    {
                        TargetValue += (target.Volume / target.Product.PackagingSize) * (target.Product.Price * target.Product.PackagingSize);
                    }
                }
                foreach (var dm in DailyMovements)
                {
                    if (product.name == dm.Product.Name)
                    {
                        ActualValue += (dm.Quantity / dm.Product.PackagingSize) * (dm.Product.Price * dm.Product.PackagingSize);
                    }
                }
                ProductGraph = new ProductGraphModel { };
                ProductGraph.Id = i;
                ProductGraph.Name = product.name;
                ProductGraph.ActualAmount = ActualValue;
                ProductGraph.TargetAmount = TargetValue;
                i++;
                ProductsGraphs.Add(ProductGraph);
            }
            List<object> data = new List<object>();
            List<string> products = new List<string>();
            List<decimal> targetvalues = new List<decimal>();
            List<decimal> actualvalues = new List<decimal>();
            foreach (var graph in ProductsGraphs.OrderByDescending(g => g.TargetAmount))
            {
                products.Add(graph.Name);
                targetvalues.Add(graph.TargetAmount);
                actualvalues.Add(graph.ActualAmount);
            }
            data.Add(products);
            data.Add(targetvalues);
            data.Add(actualvalues);
            return new JsonResult(data);
        }
    }
}
