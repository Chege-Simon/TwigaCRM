namespace TwigaCRM.Models
{
    public class CropAndAnimalPestAndDisease:TimeStamps
    {
        public int Id { get; set; }
        public int CropAndAnimalId { get; set; }
        public CropAndAnimal CropAndAnimal { get; set; }
        public int PestAndDiseaseId { get; set; }
        public PestAndDisease PestAndDisease { get; set; }
        public List<DemoDetail> DemoProducts { get; set; }
    }
}
