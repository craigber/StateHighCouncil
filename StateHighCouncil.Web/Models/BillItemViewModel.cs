using System.Security.Policy;

namespace StateHighCouncil.Web.Models
{
    public class BillItemViewModel
    {
        public int Id { get; set; }
        public string StateId { get; set; }
        public string Version { get; set; }
        public string ShortTitle { get; set; }
        public int SponsorId { get; set; }
        public string SponsorName { get; set; }
        public string SponsorImageUrl { get; set; }
        public string SponsorParty { get; set; }
        public string SponsorReligion { get; set; }
        public string SponsorProfession { get; set; }
        public int SponsorDistrict { get; set; }
        public string SponsorCounties { get; set; }
        public int? FloorSponsorId { get; set; }
        public string FloorSponsorName { get; set; }
        public string FloorSponsorImageUrl { get; set; }
        public string FloorSponsorParty { get; set; }
        public string FloorSponsorReligion { get; set; }
        public string FloorSponsorProfession { get; set; }
        public int? FloorSponsorDistrict { get; set; }
        public string FloorSponsorCounties { get; set; }
        public string GeneralProvisions { get; set; }
        public string HilightedProvisions { get; set; } 
        public string LastAction { get; set; }
        public string LastActionOwner { get; set; }
        public DateTime LastActionTime { get; set; }
        public string BillUrl { get; set; }
        public bool IsTracked { get; set; }
        public string Status { get; set; }
        public DateTime? WhenPassed {  get; set; }
        public string Subjects { get; set; }
        public string? PlusMinus { get; set; }
        public string? Commentary { get; set; }
    }
}
