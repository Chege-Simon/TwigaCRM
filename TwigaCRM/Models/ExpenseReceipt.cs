namespace TwigaCRM.Models
{
    public class ExpenseReceipt:TimeStamps
    {
        public int Id { get; set; }
        public string DocumentUrl { get; set; }
        public int RequestedExpenseId { get; set; }
        public RequestedExpense RequestedExpense { get; set; }
    }
}
