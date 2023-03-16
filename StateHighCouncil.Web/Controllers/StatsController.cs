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
        public StatsController(DataContext context, IStatsService statsService)
        {
            _context = context;
            _statsService = statsService;
        }

        public JsonResult BillCountsByParty()
        {
            var counts = _statsService.BillCountsByParty();
            var retVal = JsonSerializer.Serialize(counts).ToString();

            return Json(counts);
        }
    }
}
