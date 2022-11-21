namespace TwigaCRM.Models
{
    public class Town : TimeStamps
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
        public List<AppUser> Users { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Zone> Zones { get; set; }
        public List<CampaignBudget> CampaignBudgets { get; set; }
        public List<Call> Calls { get; set; }
    }
}
