namespace TwigaCRM.Models
{
    public class Campaign : TimeStamps
    {

        public int Id { get; set; }
        public string Title { get; set; } 
        public string Description { get; set; }
        public string SalesPersonId { get; set; }
        public AppUser SalesPerson { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfFarmers { get; set; }
        public int? PartneringStockistId { get; set; }
        public Customer PartneringStockist { get; set; }
        public string CampaignType { get; set; }
        public bool IsBudgeted { get; set; }
        public int? CampaignBudgetId { get; set; }
        public CampaignBudget CampaignBudget { get; set; }
        public bool IsSubmitted { get; set; }
        public string HRMstatus { get; set; }
        public string FOAstatus { get; set; }
        public string Status { get; set; }// in open, closed
        public string Remarks { get; set; }
        public List<RequestedExpense> RequestedExpenses { get; set; }
        public List<RequestedCampaignItem> RequestedCampaignItems { get; set; }
        public List<RequestedProduct> RequestedProducts { get; set; }
        
    }
}
