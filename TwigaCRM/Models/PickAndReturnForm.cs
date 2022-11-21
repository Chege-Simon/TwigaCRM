namespace TwigaCRM.Models
{
    public class PickAndReturnForm : TimeStamps
    {
        public int Id { get; set; }
        public int MainDistributorId { get; set; }
        public Customer MainDistributor { get; set; }
        public DateTime PickDate { get; set; }
        public string VSAId { get; set; }
        public AppUser VSA { get; set; }
        public bool IsSubmitted { get; set; }
        public string FOAstatus { get; set; }
        public string TLstatus { get; set; }
        public string FormUrl { get; set; }
        public List<PickAndReturnProduct> PickAndReturnProducts { get; set; }
    }
}
