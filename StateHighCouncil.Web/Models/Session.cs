using System.Diagnostics.Eventing.Reader;

namespace StateHighCouncil.Web.Models
{
    public class Session
    {
        public int Id { get; set; }
        public string StateId { get; set; }
        public string Name { get; set; }
        public bool IsCurrent { get; set; }
        public bool IsSelected { get; set; }
        public DateTime WhenBegin { get; set; }
        public DateTime WhenEnd { get; set; }
    }
}
