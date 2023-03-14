using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StateHighCouncil.Web.Data.Models;
using System.Diagnostics;
using StateHighCouncil.WebDataUpdater;
using ApiLegislators = StateHighCouncil.WebDataUpdater.Data.Legislators;
using StateHighCouncil.Web.Data;
using StateHighCouncil.Web.WebUpdater.Services;
using SQLitePCL;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.IdentityModel.Tokens;
using StateHighCouncil.Web.Services;
using StateHighCouncil.Web.Models;

namespace StateHighCouncil.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUpdateOrchestrator _updater;
    private readonly IAlertService _alertService;
    private readonly IStatsService _statsService;

    public HomeController(ILogger<HomeController> logger, 
        IUpdateOrchestrator updater,
        IAlertService alertService,
        IStatsService statsService)  
    {
        _logger = logger;
        _updater = updater;
        _alertService = alertService;
        _statsService = statsService;
    }

    public IActionResult Index()
    {
        ViewData["SessionMessage"] = _alertService.GetSessionMessage();

        var viewModel = new DashboardViewModel();

        viewModel.DemBillCount = _statsService.CountByParty("D");
        viewModel.RepBillCount = _statsService.CountByParty("R");
        viewModel.Top10Legislators = _statsService.TopNLegislators(10);
        viewModel.Top10Subjects = _statsService.TopNSubjects(10);
        
        return View(viewModel);
    }   

    public async Task<IActionResult> UpdateStateData()
    {
        await _updater.UpdateAsync();

        return View();
    }

    private string CalculateReligion(ApiLegislators.Legislator person)
    {
        if (!string.IsNullOrEmpty(person.bio))
        {
            var bio = person.bio.ToLower();

            if (bio.Contains("byu")
            || bio.Contains("brigham young university")
            || bio.Contains("latter day")
            || bio.Contains("latter-day"))
            {
                return "LDS";
            }
        }

        if (!string.IsNullOrEmpty(person.education))
        {
            var education = person.education.ToLower();

            if (education.Contains("byu")
            || education.Contains("brigham young university")
            || education.Contains("latter day")
            || education.Contains("latter-day"))
            {
                return "LDS";
            }
        }

        if (!string.IsNullOrEmpty(person.professionalAffiliations))
        {
            var affiliations = person.professionalAffiliations.ToLower();

            if (affiliations.Contains("byu")
            || affiliations.Contains("brigham young university")
            || affiliations.Contains("latter day")
            || affiliations.Contains("latter-day"))
            {
                return "LDS";
            }
        }
        return "";
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}