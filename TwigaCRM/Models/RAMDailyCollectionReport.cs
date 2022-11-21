namespace TwigaCRM.Models
{
    public class RAMDailyCollectionReport:TimeStamps
    {
        public int Id { get; set; }
        public string RAMId { get; set; }
        public AppUser RAM { get; set; }
        public DateTime SalesDate { get; set; }
        public bool IsSubmitted { get; set; }
        public string ApprovalStatus { get; set; }
        public List<RAMDailyCollection> RAMDailyCollections { get; set; }
    }
}
