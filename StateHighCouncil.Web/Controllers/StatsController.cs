using Microsoft.AspNetCore.Mvc;
using StateHighCouncil.Web.Data;
using StateHighCouncil.Web.Services;
using System.Text.Json;

namespace StateHighCouncil.Web.Controllers
{
    [Route("/api/stats/[action]")]
    public class StatsController : Controller
    {
        private readonly DataContext _context;
        private readonly IStatsService _statsService;
        private readonly IAlertService _alertService;

        public StatsController(DataContext context,
            IStatsService statsService,
            IAlertService alertService)
        {
            _context = context;
            _statsService = statsService;
            _alertService = alertService;
        }

        public IActionResult Index()
        {
            ViewData["SessionMessage"] = _alertService.GetSessionMessage();

            return View();
        }
        public JsonResult BillCountsByParty()
        {
            var counts = _statsService.BillCountsByParty();
            
            return Json(counts);
        }

        [HttpGet]
        public JsonResult TopNLegislators(int count)
        {
            if (count < 0) { return null; }
            var counts = _statsService.TopNLegislators(count);
            return Json(counts);
        }
        
        [HttpGet]
        public JsonResult TopNSubjects(int count)
        {
            if (count < 0) return null;
            var counts = _statsService.TopNSubjects(count);
            return Json(counts);
        }

        [HttpGet]
        public JsonResult LegislatorsByParty()
        {
            var counts = _statsService.LegislatorsByParty();
            return Json(counts);
        }
    }
}
