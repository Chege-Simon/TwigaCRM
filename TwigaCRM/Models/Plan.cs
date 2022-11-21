namespace TwigaCRM.Models
{
    public class Plan:TimeStamps
    {
        public int Id { get; set; }
        public string SalesPersonId { get; set; }
        public AppUser SalesPerson { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; }
        public bool IsSubmitted { get; set; }
        public string FOAstatus { get; set; }
        public string Remarks { get; set; }
        public List<Route> Routes { get; set; }
    }
}
