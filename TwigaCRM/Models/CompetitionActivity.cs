namespace TwigaCRM.Models
{
    public class CompetitionActivity:TimeStamps
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public string Company { get; set; }
        public string NewProduct { get; set; }
        public string NewPack { get; set; }
        public string PriceChanges { get; set; }
        public string ChangeInDeployment { get; set; }
        public string Launches { get; set; }
        public string TrainingFarmers { get; set; }
        public string TrainingStockists { get; set; }
        public string Offers { get; set; }
        public string MediaPresence { get; set; }
        public string StockOuts { get; set; }
        public string RebatsAndSchemes { get; set; }
        public string MajorPurchaseFromCompetition { get; set; }
    }
}
