namespace TwigaCRM.Models
{
    public class Call : TimeStamps
    {
        public int Id { get; set; }
        public string SpokenToId { get; set; }
        public AppUser SpokenTo { get; set; }
        public DateTime CallTime { get; set; }
        public int CallTypeId { get; set; }
        public CallType CallType { get; set; }
        public bool ContectIsCustomer { get; set; }
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string MobileNumber { get; set; }
        //only for non customers
        public string ContactCategory { get; set; } //company or individual
        public string ContactType { get; set; } //farm, agrovet, vet
        public string NonCustomerContactName { get; set; }
        public int? NonCustomerTownId { get; set; }
        public Town NonCustomerTown { get; set; }
        //continue for all
        public string Subject { get; set; }
        public string Remarks { get; set; }
        public string ForwardedToId { get; set; }
        public string Status { get; set; } //open or closed
        public List<QuestionResponse> QuestionResponses { get; set; }
    }
}
