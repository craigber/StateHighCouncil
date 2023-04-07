using System.Drawing;

namespace StateHighCouncil.Web.Models;

public class BillUpdateViewModel
{
    public string SelectedStatus { get; set; }
    public string SelectedSubject { get; set; }
    public string ShouldClearAllStatus { get; set; }
    public string ShouldClearAllFilters { get; set; }
}
