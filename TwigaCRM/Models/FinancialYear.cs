namespace TwigaCRM.Models
{
    public class FinancialYear: TimeStamps
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<SalesMovement> SalesMovements { get; set; }
        public List<CampaignBudget> CampaignBudgets { get; set; }
    }
}
