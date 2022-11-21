namespace TwigaCRM.Models
{
    public class Target : TimeStamps
    {
        public int Id { get; set; }
        public int SalesMovementId { get; set; }
        public SalesMovement SalesMovement { get; set; }
        public int? TargetCustomerId { get; set; }
        public Customer TargetCustomer { get; set; }
        public int CropAndAnimalId { get; set; }
        public CropAndAnimal CropAndAnimal { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal CropAndAnimalQuantity { get; set; }
        public decimal Volume { get; set; }
        public decimal BusinessPotential { get; set; }
        public decimal Value { get; set; }
        public decimal MarketShare { get; set; }
    }
}
