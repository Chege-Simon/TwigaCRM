namespace TwigaCRM.Models
{
    public class PickAndReturnProduct : TimeStamps
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int PickAndReturnFormId { get; set; }
        public PickAndReturnForm PickAndReturnForm { get; set; }
        public decimal PickedQuantity { get; set; }
        public decimal PickedVolume { get; set; }
        public decimal PickedValue { get; set; }
        public decimal ReturnedQuantity { get; set; }
        public decimal ReturnedVolume { get; set; }
        public decimal ReturnedValue { get; set; }
    }
}
