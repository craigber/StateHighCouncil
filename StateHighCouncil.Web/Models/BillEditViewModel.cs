using System.Security.Policy;

namespace StateHighCouncil.Web.Models
{
    public class BillEditViewModel
    {
        public int Id { get; set; }
        public string PlusMinus { get; set; }
        public string Commentary { get; set; }
        public bool IsTracked { get; set; }
        public string Status { get; set; }
    }
}
