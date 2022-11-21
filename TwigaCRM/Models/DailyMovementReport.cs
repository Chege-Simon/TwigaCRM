namespace TwigaCRM.Models
{
    public class DailyMovementReport: TimeStamps
    {
        public int Id { get; set; }
        public string SalesPersonId { get; set; }
        public AppUser SalesPerson { get; set; }
        public DateTime SalesDate { get; set; }
        public bool IsSubmitted { get; set; }
        public string FOAstatus { get; set; }
        public string TLstatus { get; set; }
        public List<DailyMovement> DailyMovements { get; set; }
    }
}
