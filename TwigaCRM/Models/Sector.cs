namespace TwigaCRM.Models
{
    public class Sector:TimeStamps
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string NormalizedName { get; set; }
        public List<UserSector> UserSectors { get; set; }
        public List<CustomerSector> CustomerSectors { get; set; }
        public List<CropAndAnimal> CropsAndAnimals { get; set; }
    }
}
