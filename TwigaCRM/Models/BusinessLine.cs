namespace TwigaCRM.Models
{
    public class BusinessLine:TimeStamps
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string NormalizedName { get; set; }
        public List<Product> Products { get; set; }
        public List<UserBusinessLine> UserBusinessLines { get; set; }
        public List<CustomerBusinessLine> CustomerBusinessLines { get; set; }
        public List<CampaignBudget> CampaignBudgets { get; set; }
    }
}
