namespace TwigaCRM.Models
{
    public class Region : TimeStamps
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Town> Towns { get; set; }
    }
}
