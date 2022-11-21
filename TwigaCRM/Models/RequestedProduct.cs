namespace TwigaCRM.Models
{
    public class RequestedProduct:TimeStamps
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal CurrentMovement { get; set; }
        public decimal CurrentMovementValue { get; set; }
        public decimal ProjectedMovement { get; set; }
        public decimal ProjectedMovementValue { get; set; }
        public decimal ActualMovement { get; set; }
        public string ImportantObservation { get; set; }
        public string FollowUpAction { get; set; }
        public bool IsFOAApproved { get; set; }
    }
}
