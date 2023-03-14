using StateHighCouncil.Web.Models;

namespace StateHighCouncil.Web.Services
{
    public interface IStatsService
    {
        public StatsTotalItem CountByParty(string party);
        public List<StatsTotalItem> TopNLegislators(int count);
        public List<StatsTotalItem> TopNSubjects(int count);
    }
}
