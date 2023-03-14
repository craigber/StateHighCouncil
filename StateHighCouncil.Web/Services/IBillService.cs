using StateHighCouncil.Web.Models;

namespace StateHighCouncil.Web.Services
{
    public interface IBillService
    {
        public IEnumerable<BillViewModel> GetBills(string subject);
    }
}
