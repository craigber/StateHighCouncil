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

        public string BillCountsByParty()
        {
            var counts = new List<BillCountByPartyItem>();
            var first = new BillCountByPartyItem
            {
                X = "Republicans",
                Filed = _statsService.CountByParty("R").Value,
                Passed = _statsService.CountByParty("R", true).Value
            };
            counts.Add(first);

            var second = new BillCountByPartyItem
            {
                X = "Democrats",
                Filed = _statsService.CountByParty("D").Value,
                Passed = _statsService.CountByParty("D", true).Value
            };
            counts.Add(second);

            var retVal = JsonSerializer.Serialize(counts).ToString();

            return retVal;
        }
    }
}

public class BillCountByPartyItem
{
    public string X { get; set; }
    public int Filed { get; set; }
    public int Passed { get; set; }
}
