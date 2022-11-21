namespace TwigaCRM.Models
{
    public class Answer : TimeStamps
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string AnswerDesc { get; set; }
        public int DisplayOrder { get; set; }
        public List<QuestionResponse> QuestionResponses { get; set; }

    }
}
