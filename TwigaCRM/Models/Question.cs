namespace TwigaCRM.Models
{
    public class Question : TimeStamps
    {
        public int Id { get; set; }
        public string QuestionDesc { get; set; }
        public string QuestionType { get; set; }//e.i multi-select, select one, typing and Product Select
        public int CallTypeId { get; set; }
        public CallType CallType { get; set; }
        public int DisplayOrder { get; set; }
        public string MoreInfo { get; set; }
        public bool Active { get; set; }
        public List<Answer> Answers { get; set; }
        public List<QuestionResponse> QuestionResponses { get; set; }

    }
}
