namespace TwigaCRM.Models
{
    public class RAMCollectionTarget:TimeStamps
    {
        public int Id { get; set; }
        public string RAMId { get; set; }
        public AppUser RAM { get; set; }
        public int FinancialYearId { get; set; }
        public FinancialYear FinancialYear { get; set; }
        public int Month { get; set; }
        public bool IsSubmitted { get; set; }
        public string ApprovalStatus { get; set; }
        public string Remarks { get; set; }
        public List<RAMCollectionTargetMapping> RAMCollectionTargetMappings { get; set; }
    }
}
