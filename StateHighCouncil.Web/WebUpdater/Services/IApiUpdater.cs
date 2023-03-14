namespace StateHighCouncil.Web.WebUpdater.Services
{
    public interface IApiUpdater
    {
        public Task<int> UpdateAsync();
    }
}
