using Microsoft.AspNetCore.Identity;

namespace TwigaCRM.Models
{
    public class AppUser:IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Station { get; set; }
        public bool IsActivated { get; set; }

        //Navigation property
        public int UserAppRoleId { get; set; }

        [PersonalData]
        public AppRole AppRole { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public int? TownId { get; set; }
        public Town Town { get; set; }

        public List<UserBusinessLine> UserBusinessLines { get; set; }
        public List<UserSector> UserSectors { get; set; }
        public List<DailyMovement> DailyMovements { get; set; }
        public List<SalesMovement> SalesMovements { get; set; }
        public List<Campaign> Campaigns { get; set; }
        public List<StockTake> StockTakes { get; set; }
        public List<Demo> Demos { get; set; }
        public List<Plan> Plans { get; set; }
        public List<Call> Calls { get; set; }
        public List<RAMPlan> RAMPlans { get; set; }
        public List<CompetitionActivity> CompetitionActivities { get; set; }
        public List<RAMFieldReport> RAMFieldReports { get; set; }
        public List<RAMDailySaleReport> RAMDailySaleReports { get; set; }
        public List<RAMSaleTarget> RAMSaleTargets { get; set; }
        public List<RAMDailyCollectionReport> RAMDailyCollectionReports { get; set; }
        public List<RAMCollectionTarget> RAMCollectionTargets { get; set; }
        public List<PickAndReturnForm> PickAndReturnForms { get; set; }

    }
}
