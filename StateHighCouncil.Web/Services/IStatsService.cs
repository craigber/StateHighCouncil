using StateHighCouncil.Web.Models.Stats;

namespace StateHighCouncil.Web.Services
{
    public interface IStatsService
    {
        public TotalsByParty BillCountsByParty();
        public List<StatsItem> TopNLegislators(int count);
        public List<StatsItem> TopNSubjects(int count);

        public List<StatsItem> LegislatorsByParty();
    }
}
