namespace TwigaCRM.Models
{
    public class Zone: TimeStamps
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TownId { get; set; }
        public Town Town { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Route> Routes { get; set; }
        public List<RAMRoute> RAMRoutes { get; set; }
    }
}
