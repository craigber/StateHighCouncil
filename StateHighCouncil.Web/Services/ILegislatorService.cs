using StateHighCouncil.Web.Models;

namespace StateHighCouncil.Web.Services
{
    public interface ILegislatorService
    {
        public Task<List<LegislatorListViewModel>> GetLegislatorsListAsync(string house);
        public Task<LegislatorViewModel> GetLegislatorAsync(int id);
    }
}
