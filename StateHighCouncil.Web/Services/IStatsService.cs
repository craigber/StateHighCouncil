using StateHighCouncil.Web.Models.Stats;

namespace StateHighCouncil.Web.Services
{
    public interface IStatsService
    {
        public TotalsByParty BillCountsByParty();
        //public List<StatsTotalItem> TopNLegislators(int count);
        //public List<StatsTotalItem> TopNSubjects(int count);
    }
}
