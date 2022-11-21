namespace TwigaCRM.Models
{
    public class CustomerBusinessLine: TimeStamps
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int BusinessLineId { get; set; }
        public BusinessLine BusinessLine { get; set; }
    }
}
