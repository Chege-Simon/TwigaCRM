namespace TwigaCRM.Models
{
    public class PestAndDisease : TimeStamps
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CropAndAnimalPestAndDisease> CropsAndAnimalsPestsAndDiseases { get; set; }

    }
}
