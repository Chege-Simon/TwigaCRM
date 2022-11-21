namespace TwigaCRM.Models
{
    public class SalesMovement : TimeStamps
    {
        public int Id { get; set; }
        public int FinancialYearId { get; set; }
        public FinancialYear FinancialYear { get; set; }
        public string SalesPersonId { get; set; }
        public AppUser SalesPerson { get; set; }
        public int Month { get; set; }
        public bool IsSubmitted { get; set; }
        public string TLstatus { get; set; }
        public string Remarks { get; set; }
        public string CreatorId { get; set; }
        public List<Target> Targets { get; set; }
    }
}
