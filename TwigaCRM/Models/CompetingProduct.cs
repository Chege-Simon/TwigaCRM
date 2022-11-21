namespace TwigaCRM.Models
{
    public class CompetingProduct:TimeStamps
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
        public List<ProductCompetingProduct> ProductCompetingProducts { get; set; }
        public List<DemoDetail> DemoProducts { get; set; }
    }
}
