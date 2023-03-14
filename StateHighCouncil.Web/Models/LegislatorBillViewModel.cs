namespace StateHighCouncil.Web.Models;

public class LegislatorBillViewModel
{
    public int Id { get; set; }
    public string Number { get; set; }
    public string Sponsor { get; set; }
    public string Title { get; set; }
    public string LastAction { get; set; }
    public DateTime LastActionTime { get; set; }
    public string Status { get; set; }
    public bool IsTracked { get; set; }
    public string BillUrl { get; set; }
    public DateTime WhenPassed { get; set; }
    public string GeneralProvisions { get; set; }
}
