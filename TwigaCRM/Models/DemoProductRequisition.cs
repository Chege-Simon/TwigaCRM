namespace TwigaCRM.Models
{
    public class DemoProductRequisition : TimeStamps
    {
        public int Id { get; set; }
        public int DemoId { get; set; }
        public Demo Demo { get; set; }
        public int StockistId { get; set; }
        public Customer Stockist { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal Quanitity { get; set; }
        public decimal TotalPrice { get; set; }
        public string InvoiceUrl { get; set; }
    }
}
