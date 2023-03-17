using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using StateHighCouncil.Web.Data;
using StateHighCouncil.Web.Models;
using StateHighCouncil.Web.Models.Stats;
using System.Collections.Generic;

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

    public List<StatsItem> TopNLegislators(int count)
    {
        count = count == 0 ? _legislators.Count() : count;

        var bills = _bills
            .GroupBy(b => b.SponsorId)
            .Select(x => new
            {
                SponsorId = x.Key,
                Count = x.Count()
            }).OrderByDescending(c => c.Count)
            .ToList();

        var totals = new List<StatsItem>();

        for (int i = 0; i < count; i++)
        {
            var currentLeg = _legislators.FirstOrDefault(l => l.Id == bills[i].SponsorId);
            var item = new StatsItem
            {

                Description = currentLeg.Name + " (" + currentLeg.Party + ")",
                Party = currentLeg.Party,
                Value = bills[i].Count                
            };
            totals.Add(item);
        }
        return totals;
    }

public List<StatsItem> TopNSubjects(int count)
    { 
        if (count < 0) throw new ArgumentOutOfRangeException("count");
        
        var totals = new List<StatsItem>();

        var subjects = _context.Subjects
            .Where(s => s.SessionStateId == _selectedSession.StateId)
            .GroupBy(s => s.Value)
            .Select(x => new StatsItem
            {
                Description = x.Key,
                Value = x.Count()
            }).OrderByDescending(o => o.Value)
            .ToList();

        if (count > 0 && count < subjects.Count())
        {
            subjects = subjects.Take(count).ToList();
        }
        return subjects;
    }

    public List<StatsItem> LegislatorsByParty()
    {
        var legs = _legislators
            .GroupBy(g => new { g.Party, g.House })
            .Select(x => new StatsItem
            {
                Description = x.Key.House,
                Party = x.Key.Party,
                Value = x.Count()
            })
            .OrderBy(o => o.Party)
            .ThenBy(o => o.Description)
            .ToList();

        return legs;
    }
}

