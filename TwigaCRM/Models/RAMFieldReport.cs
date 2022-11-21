namespace TwigaCRM.Models
{
    public class RAMFieldReport : TimeStamps
    {
        public int Id { get; set; }
        public string RAMId { get; set; }
        public AppUser RAM { get; set; }
        public DateTime RecordDate { get; set; }
        public string CustomerRelatedIssues { get; set; }
        public string WeatherAndCropSituationUpdate { get; set; }
        public string ProductRelatedIssues { get; set; }
        public string Opportunities { get; set; }
        public string OtherRemarks { get; set; }
    }
}
