using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using StateHighCouncil.Web.Data;
using StateHighCouncil.Web.Models;
using StateHighCouncil.Web.Models.Stats;

namespace StateHighCouncil.Web.Services;

public class StatsService : IStatsService
{
    private readonly DataContext _context;
    private readonly Session _selectedSession;
    private readonly List<Bill> _bills;
    private readonly List<Legislator> _legislators;

    public StatsService(DataContext context)
    {
        _context = context;
        _selectedSession = _context.Sessions.Where(s => s.IsSelected).FirstOrDefault();
        _legislators = _context.Legislators
            .Where(l => l.Session == _selectedSession.StateId)
            .ToList();
        _bills = context.Bills
            .Where(b => b.Session == _selectedSession.StateId)
            .Include(s => s.Subjects)
            .ToList();
    }

    public TotalsByParty BillCountsByParty()
    {
        var totals = new TotalsByParty
        {
            Republican = new List<StatsItem>(),
            Democrat = new List<StatsItem>()
        };

        var repListed = new StatsItem { Description = "Listed", Value = 0 };
        var repPassed = new StatsItem { Description = "Passed", Value = 0 };
        var demListed = new StatsItem { Description = "Listed", Value = 0 };
        var demPassed = new StatsItem { Description = "Passed", Value = 0 }; ;
        
        foreach(var bill in _bills)
        {
            var legislator = _legislators.FirstOrDefault(l => l.Id == bill.SponsorId);
            if (legislator.Party == "R")
            {
                repListed.Value++;
                repPassed.Value += (bill.WhenPassed > new DateTime(1, 1, 1) ? 1 : 0);
            }
            else //if(legislator.Party == "D")
            {
                demListed.Value++;
                demPassed.Value += (bill.WhenPassed > new DateTime(1, 1, 1) ? 1 : 0);
            }

        }

        totals.Republican.Add(repListed);
        totals.Republican.Add(repPassed);
        totals.Democrat.Add(demListed);
        totals.Democrat.Add(demPassed);

        return totals;
    }

    //public List<StatsTotalItem> TopNLegislators(int count)
    //{
    //    count = count == 0 ? _legislators.Count() : count;

    //    var bills = _bills.GroupBy(b => b.SponsorId)
    //        .Select(x => new
    //        {
    //            SponsorId = x.Key,
    //            Count = x.Count()
    //        }).OrderByDescending(c => c.Count)
    //        .ToList();

    //    var totals = new List<StatsTotalItem>();

    //    for (int i = 0; i < count; i++)
    //    {
    //        var item = new StatsTotalItem
    //        {
    //            Description = _legislators
    //                .FirstOrDefault(l => l.Id == bills[i].SponsorId).Name,
    //            Value = bills[i].Count
    //        };
    //        totals.Add(item);
    //    }
    //    return totals;
    //}

    //public List<StatsTotalItem> TopNSubjects(int count)
    //{
    //    var totals = new List<StatsTotalItem>();

    //    foreach(var b in _bills)
    //    {
    //        foreach(var s in b.Subjects)
    //        {
    //            var total = totals.FirstOrDefault(v => v.Description == s.Value);
    //            if (total == null)
    //            {
    //                total = new StatsTotalItem
    //                {
    //                    Description = s.Value,
    //                    Value = 1
    //                };
    //                totals.Add(total);
    //            }
    //            else
    //            {
    //                total.Value++;
    //            }
    //        }
    //    }
    //    count = count == 0 ? totals.Count() : count;
    //    var retVal = totals
    //        .OrderByDescending(t => t.Value)
    //        .Take(count)
    //        .ToList();
    //    return retVal;
    //}
}