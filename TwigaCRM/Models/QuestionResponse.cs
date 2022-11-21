namespace TwigaCRM.Models
{
    public class QuestionResponse : TimeStamps
    {
        public int Id { get; set; }
        public int? CallId { get; set; }
        public Call Call { get; set; }
        public int? QuestionId { get; set; }
        public Question Question { get; set; }
        public int? AnswerId { get; set; }
        public Answer Answer { get; set; }
        public string AnswerDesc { get; set; }
    }
}
