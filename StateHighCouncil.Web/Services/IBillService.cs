using StateHighCouncil.Web.Models;

namespace StateHighCouncil.Web.Services
{
    public interface IBillService
    {
        public Task<BillViewModel> GetBillsAsync(string subject, string status);
        public Task<BillViewModel> UpdateBillAsync(BillUpdateViewModel model);

    }
}
