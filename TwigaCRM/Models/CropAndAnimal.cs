namespace TwigaCRM.Models
{
    public class CropAndAnimal:TimeStamps
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        //navigation properties
        public int SectorId { get; set; }
        public Sector Sector { get; set; }
        public List<CropAndAnimalPestAndDisease> CropsAndAnimalsPestsAndDiseases { get; set; }
        public List<ProductCropAndAnimal> ProductsCropsAndAnimals { get; set; }
        public List<Target> Targets { get; set; }
        public List<DemoDetail> DemoProducts { get; set; }

    }
}
