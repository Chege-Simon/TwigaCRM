namespace TwigaCRM.Models
{
    public class CallType:TimeStamps
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Call> Calls { get; set; }
        public List<Question> Questions { get; set; }

    }
}
