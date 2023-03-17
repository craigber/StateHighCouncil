namespace StateHighCouncil.Web.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int BillId { get; set; } 
        public string SessionStateId { get; set; }
    }
}
