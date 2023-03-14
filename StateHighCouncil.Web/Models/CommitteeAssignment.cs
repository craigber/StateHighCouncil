namespace StateHighCouncil.Web.Models
{
    public class CommitteeAssignment
    {
        public int Id { set; get; }
        public int LegislatorId { get; set; }
        public int CommitteeId { get; set; }
    }
}
