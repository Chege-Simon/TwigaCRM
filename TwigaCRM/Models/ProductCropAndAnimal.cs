namespace TwigaCRM.Models
{
    public class ProductCropAndAnimal : TimeStamps
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CropAndAnimalId { get; set; }
        public CropAndAnimal CropAndAnimal { get; set; }
        public decimal Servings { get; set; }
        public decimal? NoOfTimeApplied { get; set; }

    }
}
