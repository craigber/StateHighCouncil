using System.Security.Policy;
using StateHighCouncil.Web.Models.Stats;

namespace StateHighCouncil.Web.Models
{
    public class DashboardViewModel
    {
        public StatsItem DemBillCount { get; set; }
        public StatsItem RepBillCount { get; set; }
        public List<StatsItem> Top10Legislators { get; set; }
        public List<StatsItem> Top10Subjects { get; set; }
    }
}
