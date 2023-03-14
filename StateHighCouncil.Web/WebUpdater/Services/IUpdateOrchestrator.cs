namespace StateHighCouncil.Web.WebUpdater.Services;

public interface IUpdateOrchestrator
{
    Task<int> UpdateAsync();
}

