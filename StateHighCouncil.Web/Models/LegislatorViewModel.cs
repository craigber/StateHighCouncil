using Microsoft.AspNetCore.Http.HttpResults;

namespace StateHighCouncil.Web.Models;

public class LegislatorViewModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? StateId { get; set; }
    public string? ImageUrl { get; set; }
    public string? House { get; set; }
    public string? Party { get; set; }
    public string? Position { get; set;}
    public int District { get; set; }
    public string? ServiceStart { get; set; }
    public string? Profession { get; set; }

    public string? Industry { get; set; }
    public string? ProfessionalAffiliations { get; set; }
    public string? Education { get; set; }
    public string? RecognitionsAndHonors { get; set; }
    public string? Counties { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? Cell { get; set; }
    public string? WorkPhone { get; set; }
    public virtual List<Committee> Committees { get; set; }
    public string? LegislationUrl { get; set; }
    public string? DemographicUrl { get; set; }
    public List<FinanceReport> FinanceReports { get; set; }
    public string? Religion { get; set; }
    public string? Gender { get; set; }
    public string? Status { get; set; }

    public string? TwitterUrl { get; set; }
    public string? FacebookUrl { get; set; }
    public string? HomePhone { get; set; }
    public string? Bio { get; set; }
    public string? Fax { get; set; }
    public string? InstagramUrl { get; set; }
    public DateTime WhenAdded { get; set; }
    public DateTime WhenLastUpdated { get; set; }

    public List<LegislatorBillViewModel> Bills { get; set; }
    virtual public List<Conflict> Conflicts { get; set; }
    public virtual List<SessionAssignment> Sessions { get; set; }
}








