namespace TwigaCRM.Models
{
    public class ProductCompetingProduct:TimeStamps
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CompetingProductId { get; set; }
        public CompetingProduct CompetingProduct { get; set; }

    }
}
