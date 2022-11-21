namespace TwigaCRM.Models
{
    public class Demo:TimeStamps
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SalesPersonId { get; set; }
        public AppUser SalesPerson { get; set; }
        public string DemoType { get; set; }
        public bool IsSubmitted { get; set; }
        public string FOAstatus { get; set; }
        public string PDstatus { get; set; }
        public string Status { get; set; }// in open, closed
        public string Remarks { get; set; }
        //naigation properties
        public List<DemoProductRequisition> DemoProductRequisitions { get; set; }
    }
}
