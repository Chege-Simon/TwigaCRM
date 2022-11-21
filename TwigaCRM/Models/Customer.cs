namespace TwigaCRM.Models
{
    public class Customer : TimeStamps
    {
        public int Id { get; set; }
        public string ContactPersonName { get; set; }
        public string SiteName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CustomerType { get; set; }
        // navigation properties
        public int TownId { get; set; }
        public Town Town { get; set; }
        public int? ZoneId { get; set; }
        public Zone Zone { get; set; }
        public string FarmerType { get; set; } //if customer is a farmer.
        public string FarmSize { get; set; } //if customer is a farmer.
        public List<CustomerBusinessLine> CustomerBusinessLines { get; set; }
        public List<CustomerSector> CustomerSectors { get; set; }
        public List<DailyMovement> DailyMovements { get; set; }
        public List<StockTake> StockTakes { get; set; }
        public List<Call> Calls { get; set; }
        public List<RAMDailyCollection> RAMDailyCollections { get; set; }
        public List<RAMDailySale> RAMDailySales { get; set; }
        public List<RAMCollectionTargetMapping> RAMCollectionTargetMappings { get; set; }
        public List<RAMSaleTargetMapping> RAMSaleTargetMappings { get; set; }
        public List<DemoDetail> DemoDetails { get; set; }
        public List<Target> Targets { get; set; }
        public List<DemoProductRequisition> DemoProductRequisitions { get; set; }
        public List<Campaign> Campaigns { get; set; }
        public List<PickAndReturnForm> PickAndReturnForms { get; set; }
    }
}
