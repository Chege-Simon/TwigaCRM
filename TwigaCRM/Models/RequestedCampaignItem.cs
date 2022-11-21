namespace TwigaCRM.Models
{
    public class RequestedCampaignItem:TimeStamps
    {
        public int Id { get; set; }
        public int CampaignItemId {get;set; }
        public CampaignItem CampaignItem {get;set; }
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public decimal RequestedQuantity {get;set; }
        public decimal RequestedPrice { get; set; }
        public decimal FOAApprovedQuantity { get; set; }
        public decimal FOAApprovedPrice { get; set; }
        public bool IsFOAApproved { get; set; }
    }
}
