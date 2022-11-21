namespace TwigaCRM.Models
{
    public class DemoDetail : TimeStamps
    {
        public int Id { get; set; }
        public int DemoId { get; set; }
        public Demo Demo { get; set; }
        public int CropAndAnimalId { get; set; }
        public CropAndAnimal CropAndAnimal { get; set; }
        public int PestAndDiseaseId { get; set; }
        public PestAndDisease PestAndDisease { get; set; }
        public decimal RequestedQuantityOfSample { get; set; }
        public decimal RequestedNumberOfDemos { get; set; }
        public decimal ApprovedQuantityOfSample { get; set; }
        public decimal ApprovedNumberOfDemos { get; set; }
        public decimal RequestedVolumeOfProduct { get; set; }
        public decimal ApprovedVolumeOfProduct { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CompetingProductId { get; set; }
        public CompetingProduct CompetingProduct { get; set; }
        public int? SiteId { get; set; }
        public Customer Site { get; set; }
        public string Remarks { get; set; }
        public bool IsFOAApproved { get; set; }
        public bool IsPDApproved { get; set; }
        public List<DemoProgressReport> DemoProgressReports { get; set; }
    }
}
