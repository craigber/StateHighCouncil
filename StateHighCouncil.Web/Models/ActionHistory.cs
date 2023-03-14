namespace StateHighCouncil.Web.Models;

public class ActionHistory
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Action { get; set; }
    public string Location { get; set; }
    public int BillId { get; set; }
}
