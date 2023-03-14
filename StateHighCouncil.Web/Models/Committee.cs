namespace StateHighCouncil.Web.Models
{
    public class Committee
    {
        public int Id { get; set; }
        public string StateId { get; set; }
        public string Name { get; set; }
        public string? Url { get; set; }
    }
}
