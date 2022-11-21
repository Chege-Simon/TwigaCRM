namespace TwigaCRM.Models
{
    public class Product: TimeStamps
    {
        public int Id { get; set; }
        public string Name { get; set; }   
        public string  Description { get; set; }
        public string Code { get; set;}
        public decimal Price { get; set;}
        public string UnitOfMeasure { get; set; }
        public decimal PackagingSize { get; set; }
        public int BusinessLineId { get; set; }
        public BusinessLine BusinessLine { get; set; }
        public List<ProductCropAndAnimal> ProductsCropsAndAnimals { get; set; }
        public List<DailyMovement> DailyMovements { get; set; }
        public List<Target> Targets { get; set; }
        public List<RequestedProduct> RequestedProducts { get; set; }
        public List<StockTake> StockTakes { get; set; }
        public List<ProductCompetingProduct> ProductCompetingProducts { get; set; }
        public List<DemoDetail> DemoProducts { get; set; }
        public List<DemoProductRequisition> DemoProductRequisitions { get; set; }
    }
}
