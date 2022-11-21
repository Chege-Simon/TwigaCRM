namespace TwigaCRM.Models
{
    public class CampaignBudget: TimeStamps
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int FinancialYearId { get; set; }
        public FinancialYear FinancialYear { get; set; }
        public int BusinessLineId { get; set; }
        public BusinessLine BusinessLine { get; set; }
        public int Month { get; set;}
        public decimal Amount { get; set; }
        public List<Campaign> Campaigns { get; set; }
    }
}
