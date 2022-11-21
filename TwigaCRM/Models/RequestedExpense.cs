namespace TwigaCRM.Models
{
    public class RequestedExpense:TimeStamps
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public int ExpenseCategoryId { get; set; }
        public ExpenseCategory ExpenseCategory { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal ApprovedAmount { get; set; }
        public bool IsFOAApproved { get; set; }
        public List<ExpenseReceipt> ExpenseReceipts { get; set; }
    }
}
