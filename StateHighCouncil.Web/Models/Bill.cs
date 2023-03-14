using System.Drawing;
using System.Security.Policy;

namespace StateHighCouncil.Web.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public string StateId { get; set; }
        public string Version { get; set; }
        public string ShortTitle { get; set; }
        public int SponsorId { get; set; }
        public string SponsorStateId { get; set; }
        public int FloorSponsorId { get; set; }
        public string FloorSponsorStateId { get; set; }
        public string GeneralProvisions { get; set; }
        public string HilightedProvisions { get; set; }
        public string Monies { get; set; }
        public string Attorney { get; set; }
        public string FiscalAnalyst { get; set; }
        public string LastAction { get; set; }
        public string LastActionOwner { get; set; }
        public DateTime LastActionTime { get; set; }
        public string TrackingId { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<CodeSection> CodeSections { get; set; }
        //public List<Agenda> Agendas { get; set; }
        public bool IsTracked { get; set; }
        public string Status { get; set; }
        public DateTime WhenCreated { get; set; }
        public DateTime WhenLastUpdated { get; set; }
        public DateTime WhenPassed { get; set; }
        public string Session { get; set; }
        //public List<ActionHistory> ActionHistories { get; set; }
    }
}
