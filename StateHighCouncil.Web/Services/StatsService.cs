using Microsoft.EntityFrameworkCore;
using StateHighCouncil.Web.Data;
using StateHighCouncil.Web.Models;

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

    public StatsTotalItem CountByParty(string party)
    {
        var retVal = new StatsTotalItem
        {
            Description = party,
            Value = 0
        };

        foreach (var l in _legislators.Where(l => l.Party == party))
        {
            if (l.Party == party)
            {
                retVal.Value += _bills.Where(b => b.SponsorId == l.Id).Count();
            }
        }

        return retVal;
    }

    public List<StatsTotalItem> TopNLegislators(int count)
    {
        count = count == 0 ? _legislators.Count() : count;

        var bills = _bills.GroupBy(b => b.SponsorId)
            .Select(x => new
            {
                SponsorId = x.Key,
                Count = x.Count()
            }).OrderByDescending(c => c.Count)
            .ToList();

        var totals = new List<StatsTotalItem>();

        for (int i = 0; i < count; i++)
        {
            var item = new StatsTotalItem
            {
                Description = _legislators
                    .FirstOrDefault(l => l.Id == bills[i].SponsorId).Name,
                Value = bills[i].Count
            };
            totals.Add(item);
        }
        return totals;
    }

    public List<StatsTotalItem> TopNSubjects(int count)
    {
        var totals = new List<StatsTotalItem>();

        foreach(var b in _bills)
        {
            foreach(var s in b.Subjects)
            {
                var total = totals.FirstOrDefault(v => v.Description == s.Value);
                if (total == null)
                {
                    total = new StatsTotalItem
                    {
                        Description = s.Value,
                        Value = 1
                    };
                    totals.Add(total);
                }
                else
                {
                    total.Value++;
                }
            }
        }
        count = count == 0 ? totals.Count() : count;
        var retVal = totals
            .OrderByDescending(t => t.Value)
            .Take(count)
            .ToList();
        return retVal;
    }
}