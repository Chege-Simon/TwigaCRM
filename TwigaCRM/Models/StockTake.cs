namespace TwigaCRM.Models
{
    public class StockTake: TimeStamps
    {
        public int Id { get; set; }
        public string RetailAccountManagerId { get; set; }
        public AppUser RetailAccountManager { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal Quantity { get; set; }
        public int MainDistributorId { get; set; }
        public Customer MainDistributor { get; set; }
        public DateTime StockTakeDate { get; set; }

    }
}
