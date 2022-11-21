namespace TwigaCRM.Models
{
    public class DemoProgressReport:TimeStamps
    {
        public int Id { get; set; }
        public string DocumentUrl { get; set; }
        public bool IsFinalReport { get; set; }
        public int DemoDetailId { get; set; }
        public DemoDetail DemoDetail { get; set; }
    }
}
