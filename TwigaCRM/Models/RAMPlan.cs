namespace TwigaCRM.Models
{
    public class RAMPlan:TimeStamps
    {
        public int Id { get; set; }
        public string RAMId { get; set; }
        public AppUser RAM { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; }
        public bool IsSubmitted { get; set; }
        public string ApprovalStatus { get; set; }
        public string Remarks { get; set; }
        public List<RAMRoute> RAMRoutes { get; set; }
    }
}
