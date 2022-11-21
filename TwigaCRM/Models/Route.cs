namespace TwigaCRM.Models
{
    public class Route : TimeStamps
    {
        public int Id { get; set; }
        public int PlanId { get; set; }
        public Plan Plan { get; set; }
        public string Day { get; set; }
        public DateTime RouteDate { get; set; }
        public int? ZoneId { get; set; }
        public Zone Zone { get; set; }
        public string Activity { get; set; }
        public string ActualLong { get; set; }
        public string ActualLat { set; get; }
        public bool IsFOAApproved { set; get; }
    }
}
