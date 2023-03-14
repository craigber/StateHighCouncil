using System.Security.Policy;

namespace StateHighCouncil.Web.Models
{
    public class DashboardViewModel
    {
        public StatsTotalItem DemBillCount { get; set; }
        public StatsTotalItem RepBillCount { get; set; }
        public List<StatsTotalItem> Top10Legislators { get; set; }
        public List<StatsTotalItem> Top10Subjects { get; set; }
    }
}
