namespace StateHighCouncil.Web.Models
{
    public class CommitteeRecommendation
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public string Committee { get; set; }
        public string Version { get; set; }
    }
}
