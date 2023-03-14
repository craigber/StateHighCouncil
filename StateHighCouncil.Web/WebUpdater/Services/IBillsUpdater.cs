namespace StateHighCouncil.Web.WebUpdater.Services
{
    public interface IBillsUpdater
    {
        public Task<int> UpdateAsync();
    }
}
