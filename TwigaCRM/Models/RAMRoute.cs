namespace TwigaCRM.Models
{
    public class RAMRoute : TimeStamps
    {
        public int Id { get; set; }
        public int RAMPlanId { get; set; }
        public RAMPlan RAMPlan { get; set; }
        public string Day { get; set; }
        public DateTime RouteDate { get; set; }
        public int? ZoneId { get; set; }
        public Zone Zone { get; set; }
        public int MainDistributorId { get; set; }
        public Customer MainDistributor { get; set; }
        public string Activity { get; set; }
        public string ActualLong { get; set; }
        public string ActualLat { set; get; }
        public bool IsApproved { set; get; }
    }
}
