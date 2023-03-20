using Microsoft.AspNetCore.Mvc.Rendering;

namespace StateHighCouncil.Web.Models
{
    public class BillViewModel
    {
        public string SelectedStatus {get; set;}
        public string SelectedSubject { get; set;}
        public SelectList Statuses { get; set; }
        public SelectList Subjects { get; set; }
        public List<BillItemViewModel> Bills { get; set; }
    }
}
