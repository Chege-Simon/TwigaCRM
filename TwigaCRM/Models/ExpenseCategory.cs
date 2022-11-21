namespace TwigaCRM.Models
{
    public class ExpenseCategory : TimeStamps
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<RequestedExpense> RequestedExpenses { get; set; }
    }
}
