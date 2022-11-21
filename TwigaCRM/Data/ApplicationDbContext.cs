using J.E.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TwigaCRM.Models;
using Route = TwigaCRM.Models.Route;

namespace TwigaCRM.Data
{
    public class ApplicationDbContext : AuditableIdentityContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is TimeStamps && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((TimeStamps)entityEntry.Entity).UpdateAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((TimeStamps)entityEntry.Entity).CreateAt = DateTime.Now;
                    ((TimeStamps)entityEntry.Entity).UpdateAt = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            var AddedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Added).ToList();
            AddedEntities.ForEach(E =>
            {
                var info = E.GetType();
                if (E.GetType().GetProperty("CreateAt") != null || E.GetType().GetProperty("UpdateAt") != null)
                {
                    E.Property("CreateAt").CurrentValue = DateTime.Now;
                    E.Property("UpdateAt").CurrentValue = DateTime.Now;

                }
            });

            var EditedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Modified).ToList();

            EditedEntities.ForEach(E =>
            {
                if (E.GetType().GetProperty("CreateAt") != null || E.GetType().GetProperty("UpdateAt") != null)
                {
                    E.Property("UpdateAt").CurrentValue = DateTime.Now;
                }
            });

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Seed();
            builder.Entity<AppUser>()
                .HasOne(b => b.AppRole)
                .WithMany(ba => ba.Users)
                .HasForeignKey(bi => bi.UserAppRoleId);

            builder.Entity<AppRole_Permission>()
                .HasOne(b => b.AppRole)
                .WithMany(ba => ba.AppRolePermissions)
                .HasForeignKey(bi => bi.AppRoleId);

            builder.Entity<AppRole_Permission>()
                .HasOne(b => b.Permission)
                .WithMany(ba => ba.AppRolePermissions)
                .HasForeignKey(bi => bi.PermissionId);

            builder.Entity<Town>()
                .HasOne(b => b.Region)
                .WithMany(ba => ba.Towns)
                .HasForeignKey(bi => bi.RegionId);

            builder.Entity<AppUser>()
                .HasOne(b => b.Town)
                .WithMany(ba => ba.Users)
                .HasForeignKey(bi => bi.TownId);

            builder.Entity<UserBusinessLine>()
                .HasOne(b => b.AppUser)
                .WithMany(ba => ba.UserBusinessLines)
                .HasForeignKey(bi => bi.AppUserId);

            builder.Entity<UserSector>()
                .HasOne(b => b.AppUser)
                .WithMany(ba => ba.UserSectors)
                .HasForeignKey(bi => bi.AppUserId);

            builder.Entity<UserBusinessLine>()
                .HasOne(b => b.BusinessLine)
                .WithMany(ba => ba.UserBusinessLines)
                .HasForeignKey(bi => bi.BusinessLineId);

            builder.Entity<UserSector>()
                .HasOne(b => b.Sector)
                .WithMany(ba => ba.UserSectors)
                .HasForeignKey(bi => bi.SectorId);

            builder.Entity<Customer>()
                .HasOne(b => b.Town)
                .WithMany(ba => ba.Customers)
                .HasForeignKey(bi => bi.TownId);

            builder.Entity<CustomerBusinessLine>()
                .HasOne(b => b.Customer)
                .WithMany(ba => ba.CustomerBusinessLines)
                .HasForeignKey(bi => bi.CustomerId);

            builder.Entity<CustomerSector>()
                .HasOne(b => b.Customer)
                .WithMany(ba => ba.CustomerSectors)
                .HasForeignKey(bi => bi.CustomerId);

            builder.Entity<CustomerBusinessLine>()
                .HasOne(b => b.BusinessLine)
                .WithMany(ba => ba.CustomerBusinessLines)
                .HasForeignKey(bi => bi.BusinessLineId);

            builder.Entity<CustomerSector>()
                .HasOne(b => b.Sector)
                .WithMany(ba => ba.CustomerSectors)
                .HasForeignKey(bi => bi.SectorId);

            builder.Entity<Product>()
                .HasOne(b => b.BusinessLine)
                .WithMany(ba => ba.Products)
                .HasForeignKey(bi => bi.BusinessLineId);

            builder.Entity<CropAndAnimal>()
                .HasOne(b => b.Sector)
                .WithMany(ba => ba.CropsAndAnimals)
                .HasForeignKey(bi => bi.SectorId);

            builder.Entity<ProductCropAndAnimal>()
                .HasOne(b => b.Product)
                .WithMany(ba => ba.ProductsCropsAndAnimals)
                .HasForeignKey(bi => bi.ProductId);

            builder.Entity<ProductCropAndAnimal>()
                .HasOne(b => b.CropAndAnimal)
                .WithMany(ba => ba.ProductsCropsAndAnimals)
                .HasForeignKey(bi => bi.CropAndAnimalId);

            builder.Entity<CropAndAnimalPestAndDisease>()
                .HasOne(b => b.CropAndAnimal)
                .WithMany(ba => ba.CropsAndAnimalsPestsAndDiseases)
                .HasForeignKey(bi => bi.CropAndAnimalId);

            builder.Entity<CropAndAnimalPestAndDisease>()
                .HasOne(b => b.PestAndDisease)
                .WithMany(ba => ba.CropsAndAnimalsPestsAndDiseases)
                .HasForeignKey(bi => bi.PestAndDiseaseId);

            builder.Entity<DailyMovement>()
                .HasOne(b => b.Product)
                .WithMany(ba => ba.DailyMovements)
                .HasForeignKey(bi => bi.ProductId);

            builder.Entity<DailyMovement>()
                .HasOne(b => b.MainDistributor)
                .WithMany(ba => ba.DailyMovements)
                .HasForeignKey(bi => bi.MainDistributorId);

            builder.Entity<DailyMovement>()
                .HasOne(b => b.DailyMovementReport)
                .WithMany(ba => ba.DailyMovements)
                .HasForeignKey(bi => bi.DailyMovementReportId);

            builder.Entity<Target>()
                .HasOne(b => b.SalesMovement)
                .WithMany(ba => ba.Targets)
                .HasForeignKey(bi => bi.SalesMovementId);

            builder.Entity<Target>()
                .HasOne(b => b.Product)
                .WithMany(ba => ba.Targets)
                .HasForeignKey(bi => bi.ProductId);

            builder.Entity<Target>()
                .HasOne(b => b.CropAndAnimal)
                .WithMany(ba => ba.Targets)
                .HasForeignKey(bi => bi.CropAndAnimalId);

            builder.Entity<SalesMovement>()
                .HasOne(b => b.SalesPerson)
                .WithMany(ba => ba.SalesMovements)
                .HasForeignKey(bi => bi.SalesPersonId);

            builder.Entity<SalesMovement>()
                .HasOne(b => b.FinancialYear)
                .WithMany(ba => ba.SalesMovements)
                .HasForeignKey(bi => bi.FinancialYearId);

            builder.Entity<RequestedExpense>()
                .HasOne(b => b.ExpenseCategory)
                .WithMany(ba => ba.RequestedExpenses)
                .HasForeignKey(bi => bi.ExpenseCategoryId);

            builder.Entity<RequestedExpense>()
                .HasOne(b => b.Campaign)
                .WithMany(ba => ba.RequestedExpenses)
                .HasForeignKey(bi => bi.CampaignId);

            builder.Entity<RequestedCampaignItem>()
                .HasOne(b => b.Campaign)
                .WithMany(ba => ba.RequestedCampaignItems)
                .HasForeignKey(bi => bi.CampaignId);

            builder.Entity<RequestedCampaignItem>()
                .HasOne(b => b.CampaignItem)
                .WithMany(ba => ba.RequestedCampaignItems)
                .HasForeignKey(bi => bi.CampaignItemId);

            builder.Entity<RequestedProduct>()
                .HasOne(b => b.Product)
                .WithMany(ba => ba.RequestedProducts)
                .HasForeignKey(bi => bi.ProductId);

            builder.Entity<RequestedProduct>()
                .HasOne(b => b.Campaign)
                .WithMany(ba => ba.RequestedProducts)
                .HasForeignKey(bi => bi.CampaignId);

            builder.Entity<Campaign>()
                .HasOne(b => b.CampaignBudget)
                .WithMany(ba => ba.Campaigns)
                .HasForeignKey(bi => bi.CampaignBudgetId);

            builder.Entity<Campaign>()
                .HasOne(b => b.SalesPerson)
                .WithMany(ba => ba.Campaigns)
                .HasForeignKey(bi => bi.SalesPersonId);

            builder.Entity<CampaignBudget>()
                .HasOne(b => b.FinancialYear)
                .WithMany(ba => ba.CampaignBudgets)
                .HasForeignKey(bi => bi.FinancialYearId);

            builder.Entity<ExpenseReceipt>()
                .HasOne(b => b.RequestedExpense)
                .WithMany(ba => ba.ExpenseReceipts)
                .HasForeignKey(bi => bi.RequestedExpenseId);

            builder.Entity<Customer>()
                .HasOne(b => b.Zone)
                .WithMany(ba => ba.Customers)
                .HasForeignKey(bi => bi.ZoneId);

            builder.Entity<Zone>()
                .HasOne(b => b.Town)
                .WithMany(ba => ba.Zones)
                .HasForeignKey(bi => bi.TownId);

            builder.Entity<Route>()
                .HasOne(b => b.Zone)
                .WithMany(ba => ba.Routes)
                .HasForeignKey(bi => bi.ZoneId);

            builder.Entity<Route>()
                .HasOne(b => b.Plan)
                .WithMany(ba => ba.Routes)
                .HasForeignKey(bi => bi.PlanId);

            builder.Entity<Plan>()
                .HasOne(b => b.SalesPerson)
                .WithMany(ba => ba.Plans)
                .HasForeignKey(bi => bi.SalesPersonId);

            builder.Entity<Call>()
                .HasOne(b => b.SpokenTo)
                .WithMany(ba => ba.Calls)
                .HasForeignKey(bi => bi.SpokenToId);

            builder.Entity<Call>()
                .HasOne(b => b.Customer)
                .WithMany(ba => ba.Calls)
                .HasForeignKey(bi => bi.CustomerId);

            builder.Entity<Call>()
                .HasOne(b => b.CallType)
                .WithMany(ba => ba.Calls)
                .HasForeignKey(bi => bi.CallTypeId);

            builder.Entity<Call>()
                .HasOne(b => b.NonCustomerTown)
                .WithMany(ba => ba.Calls)
                .HasForeignKey(bi => bi.NonCustomerTownId);

            builder.Entity<Question>()
                .HasOne(b => b.CallType)
                .WithMany(ba => ba.Questions)
                .HasForeignKey(bi => bi.CallTypeId);

            builder.Entity<Answer>()
                .HasOne(b => b.Question)
                .WithMany(ba => ba.Answers)
                .HasForeignKey(bi => bi.QuestionId);

            builder.Entity<QuestionResponse>()
                .HasOne(b => b.Question)
                .WithMany(ba => ba.QuestionResponses)
                .HasForeignKey(bi => bi.QuestionId);

            builder.Entity<QuestionResponse>()
                .HasOne(b => b.Answer)
                .WithMany(ba => ba.QuestionResponses)
                .HasForeignKey(bi => bi.AnswerId);

            builder.Entity<QuestionResponse>()
                .HasOne(b => b.Call)
                .WithMany(ba => ba.QuestionResponses)
                .HasForeignKey(bi => bi.CallId);

            builder.Entity<CompetitionActivity>()
                .HasOne(b => b.AppUser)
                .WithMany(ba => ba.CompetitionActivities)
                .HasForeignKey(bi => bi.AppUserId);

            builder.Entity<RAMRoute>()
                .HasOne(b => b.Zone)
                .WithMany(ba => ba.RAMRoutes)
                .HasForeignKey(bi => bi.ZoneId);

            builder.Entity<RAMRoute>()
                .HasOne(b => b.RAMPlan)
                .WithMany(ba => ba.RAMRoutes)
                .HasForeignKey(bi => bi.RAMPlanId);

            builder.Entity<RAMPlan>()
                .HasOne(b => b.RAM)
                .WithMany(ba => ba.RAMPlans)
                .HasForeignKey(bi => bi.RAMId);

            builder.Entity<RAMDailyCollection>()
                .HasOne(b => b.MainDistributor)
                .WithMany(ba => ba.RAMDailyCollections)
                .HasForeignKey(bi => bi.MainDistributorId);

            builder.Entity<RAMDailyCollection>()
                .HasOne(b => b.RAMDailyCollectionReport)
                .WithMany(ba => ba.RAMDailyCollections)
                .HasForeignKey(bi => bi.RAMDailyCollectionReportId);

            builder.Entity<RAMDailyCollectionReport>()
                .HasOne(b => b.RAM)
                .WithMany(ba => ba.RAMDailyCollectionReports)
                .HasForeignKey(bi => bi.RAMId);

            builder.Entity<RAMCollectionTargetMapping>()
                .HasOne(b => b.RAMCollectionTarget)
                .WithMany(ba => ba.RAMCollectionTargetMappings)
                .HasForeignKey(bi => bi.RAMCollectionTargetId);

            builder.Entity<RAMCollectionTargetMapping>()
                .HasOne(b => b.MainDistributor)
                .WithMany(ba => ba.RAMCollectionTargetMappings)
                .HasForeignKey(bi => bi.MainDistributorId);

            builder.Entity<RAMDailySale>()
                .HasOne(b => b.MainDistributor)
                .WithMany(ba => ba.RAMDailySales)
                .HasForeignKey(bi => bi.MainDistributorId);

            builder.Entity<RAMDailySale>()
                .HasOne(b => b.RAMDailySaleReport)
                .WithMany(ba => ba.RAMDailySales)
                .HasForeignKey(bi => bi.RAMDailySaleReportId);

            builder.Entity<RAMDailySaleReport>()
                .HasOne(b => b.RAM)
                .WithMany(ba => ba.RAMDailySaleReports)
                .HasForeignKey(bi => bi.RAMId);

            builder.Entity<RAMSaleTargetMapping>()
                .HasOne(b => b.RAMSaleTarget)
                .WithMany(ba => ba.RAMSaleTargetMappings)
                .HasForeignKey(bi => bi.RAMSaleTargetId);

            builder.Entity<RAMSaleTargetMapping>()
                .HasOne(b => b.MainDistributor)
                .WithMany(ba => ba.RAMSaleTargetMappings)
                .HasForeignKey(bi => bi.MainDistributorId);

            builder.Entity<RAMFieldReport>()
                .HasOne(b => b.RAM)
                .WithMany(ba => ba.RAMFieldReports)
                .HasForeignKey(bi => bi.RAMId);

            builder.Entity<RAMSaleTarget>()
                .HasOne(b => b.RAM)
                .WithMany(ba => ba.RAMSaleTargets)
                .HasForeignKey(bi => bi.RAMId);

            builder.Entity<RAMCollectionTarget>()
                .HasOne(b => b.RAM)
                .WithMany(ba => ba.RAMCollectionTargets)
                .HasForeignKey(bi => bi.RAMId);

            builder.Entity<Target>()
                .HasOne(b => b.TargetCustomer)
                .WithMany(ba => ba.Targets)
                .HasForeignKey(bi => bi.TargetCustomerId);

            builder.Entity<DemoProductRequisition>()
                .HasOne(b => b.Demo)
                .WithMany(ba => ba.DemoProductRequisitions)
                .HasForeignKey(bi => bi.DemoId);

            builder.Entity<DemoProductRequisition>()
                .HasOne(b => b.Product)
                .WithMany(ba => ba.DemoProductRequisitions)
                .HasForeignKey(bi => bi.ProductId);

            builder.Entity<DemoProductRequisition>()
                .HasOne(b => b.Stockist)
                .WithMany(ba => ba.DemoProductRequisitions)
                .HasForeignKey(bi => bi.StockistId);

            builder.Entity<DemoDetail>()
                .HasOne(b => b.Site)
                .WithMany(ba => ba.DemoDetails)
                .HasForeignKey(bi => bi.SiteId);

            builder.Entity<Campaign>()
                .HasOne(b => b.PartneringStockist)
                .WithMany(ba => ba.Campaigns)
                .HasForeignKey(bi => bi.PartneringStockistId);

            builder.Entity<CampaignBudget>()
                .HasOne(b => b.BusinessLine)
                .WithMany(ba => ba.CampaignBudgets)
                .HasForeignKey(bi => bi.BusinessLineId);

            builder.Entity<PickAndReturnForm>()
                .HasOne(b => b.MainDistributor)
                .WithMany(ba => ba.PickAndReturnForms)
                .HasForeignKey(bi => bi.MainDistributorId);

            builder.Entity<PickAndReturnForm>()
                .HasOne(b => b.VSA)
                .WithMany(ba => ba.PickAndReturnForms)
                .HasForeignKey(bi => bi.VSAId);

            builder.Entity<PickAndReturnProduct>()
                .HasOne(b => b.PickAndReturnForm)
                .WithMany(ba => ba.PickAndReturnProducts)
                .HasForeignKey(bi => bi.PickAndReturnFormId);
        }

        public DbSet<TwigaCRM.Models.AppRole> AppRole { get; set; }
        public DbSet<TwigaCRM.Models.Permission> Permission { get; set; }
        public DbSet<TwigaCRM.Models.AppRole_Permission> AppRole_Permission { get; set; }
        public DbSet<TwigaCRM.Models.Town> Town { get; set; }
        public DbSet<TwigaCRM.Models.Region> Region { get; set; }
        public DbSet<TwigaCRM.Models.Sector> Sector { get; set; }
        public DbSet<TwigaCRM.Models.BusinessLine> BusinessLine { get; set; }
        public DbSet<TwigaCRM.Models.UserBusinessLine> UserBusinessLine { get; set; }
        public DbSet<TwigaCRM.Models.UserSector> UserSector { get; set; }
        public DbSet<TwigaCRM.Models.CustomerBusinessLine> CustomerBusinessLine { get; set; }
        public DbSet<TwigaCRM.Models.CustomerSector> CustomerSector { get; set; }
        public DbSet<TwigaCRM.Models.Customer> Customer { get; set; }
        public DbSet<TwigaCRM.Models.ProductCropAndAnimal> ProductCropAndAnimal { get; set; }
        public DbSet<TwigaCRM.Models.Product> Product { get; set; }
        public DbSet<TwigaCRM.Models.PestAndDisease> PestAndDisease { get; set; }
        public DbSet<TwigaCRM.Models.CropAndAnimalPestAndDisease> CropAndAnimalPestAndDisease { get; set; }
        public DbSet<TwigaCRM.Models.CropAndAnimal> CropAndAnimal { get; set; }
        public DbSet<TwigaCRM.Models.DailyMovement> DailyMovement { get; set; }
        public DbSet<TwigaCRM.Models.DailyMovementReport> DailyMovementReport { get; set; }
        public DbSet<TwigaCRM.Models.FinancialYear> FinancialYear { get; set; }
        public DbSet<TwigaCRM.Models.SalesMovement> SalesMovement { get; set; }
        public DbSet<TwigaCRM.Models.Target> Target { get; set; }
        public DbSet<TwigaCRM.Models.CampaignBudget> CampaignBudget { get; set; }
        public DbSet<TwigaCRM.Models.CampaignItem> CampaignItem { get; set; }
        public DbSet<TwigaCRM.Models.Campaign> Campaign { get; set; }
        public DbSet<TwigaCRM.Models.ExpenseCategory> ExpenseCategory { get; set; }
        public DbSet<TwigaCRM.Models.RequestedCampaignItem> RequestedCampaignItem { get; set; }
        public DbSet<TwigaCRM.Models.RequestedExpense> RequestedExpense { get; set; }
        public DbSet<TwigaCRM.Models.RequestedProduct> RequestedProduct { get; set; }
        public DbSet<TwigaCRM.Models.ExpenseReceipt> ExpenseReceipt { get; set; }
        public DbSet<TwigaCRM.Models.Zone> Zone { get; set; }
        public DbSet<TwigaCRM.Models.StockTake> StockTake { get; set; }
        public DbSet<TwigaCRM.Models.Demo> Demo { get; set; }
        public DbSet<TwigaCRM.Models.DemoDetail> DemoDetail { get; set; }
        public DbSet<TwigaCRM.Models.DemoProgressReport> DemoProgressReport { get; set; }
        public DbSet<TwigaCRM.Models.ProductCompetingProduct> ProductCompetingProduct { get; set; }
        public DbSet<TwigaCRM.Models.CompetingProduct> CompetingProduct { get; set; }
        public DbSet<TwigaCRM.Models.Plan> Plan { get; set; }
        public DbSet<TwigaCRM.Models.Route> Route { get; set; }
        public DbSet<TwigaCRM.Models.Answer> Answer { get; set; }
        public DbSet<TwigaCRM.Models.Call> Call { get; set; }
        public DbSet<TwigaCRM.Models.CallType> CallType { get; set; }
        public DbSet<TwigaCRM.Models.Question> Question { get; set; }
        public DbSet<TwigaCRM.Models.QuestionResponse> Response { get; set; }
        public DbSet<TwigaCRM.Models.CompetitionActivity> CompetitionActivity { get; set; }
        public DbSet<TwigaCRM.Models.RAMCollectionTargetMapping> RAMCollectionTargetMapping { get; set; }
        public DbSet<TwigaCRM.Models.RAMCollectionTarget> RAMCollectionTarget { get; set; }
        public DbSet<TwigaCRM.Models.RAMDailyCollectionReport> RAMDailyCollectionReport { get; set; }
        public DbSet<TwigaCRM.Models.RAMDailyCollection> RAMDailyCollection { get; set; }
        public DbSet<TwigaCRM.Models.RAMDailySaleReport> RAMDailySaleReport { get; set; }
        public DbSet<TwigaCRM.Models.RAMDailySale> RAMDailySale { get; set; }
        public DbSet<TwigaCRM.Models.RAMFieldReport> RAMFieldReport { get; set; }
        public DbSet<TwigaCRM.Models.RAMPlan> RAMPlan { get; set; }
        public DbSet<TwigaCRM.Models.RAMRoute> RAMRoute { get; set; }
        public DbSet<TwigaCRM.Models.RAMSaleTargetMapping> RAMSaleTargetMapping { get; set; }
        public DbSet<TwigaCRM.Models.RAMSaleTarget> RAMSaleTarget { get; set; }
        public DbSet<TwigaCRM.Models.DemoProductRequisition> DemoProductRequisition { get; set; }
        public DbSet<TwigaCRM.Models.PickAndReturnForm> PickAndReturnForm { get; set; }
        public DbSet<TwigaCRM.Models.PickAndReturnProduct> PickAndReturnProduct { get; set; }
    }
}