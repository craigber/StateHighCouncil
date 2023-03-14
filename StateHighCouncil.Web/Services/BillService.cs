using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StateHighCouncil.Web.Data;
using StateHighCouncil.Web.Models;

namespace StateHighCouncil.Web.Services;

public class BillService : IBillService
{
    private readonly DataContext _context;
    private readonly IAlertService _alertService;
    private readonly Session _selectedSession;

    public BillService(DataContext context, IAlertService alertService)
    {
        _context = context;
        _alertService = alertService;
        _selectedSession = _context.Sessions.Where(s => s.IsSelected).FirstOrDefault();
    }

    public IEnumerable<BillViewModel> GetBills(string subject)
    {
        var bills = _context.Bills?
            .Where(b => b.Session == _selectedSession.StateId)
            .OrderBy(o => o.Version).ToList();
        
        if(bills == null || !bills.Any())
        {
            return null;
        }
        
        var legislators = _context.Legislators.ToList();
        var viewModel = new List<BillViewModel>();

        foreach (var bill in bills)
        {
            if (string.IsNullOrEmpty(subject)
                || bill.Subjects.Any(s => s.Value == subject))
            {
                var sponsor = legislators
                    .FirstOrDefault(l => l.Id == bill.SponsorId);

                Legislator floorSponsor = null;

                if (bill.FloorSponsorId > 0)
                {
                    floorSponsor = legislators
                        .FirstOrDefault(l => l.Id == bill.FloorSponsorId);
                }

                var vm = new BillViewModel
                {
                    Id = bill.Id,
                    StateId = bill.StateId,
                    Version = bill.Version,
                    ShortTitle = bill.ShortTitle,
                    SponsorId = bill.SponsorId,
                    SponsorName = sponsor.Name,
                    SponsorImageUrl = sponsor.ImageUrl,
                    SponsorParty = sponsor.Party,
                    SponsorReligion = sponsor.Religion,
                    SponsorProfession = sponsor.Profession,
                    SponsorDistrict = sponsor.District,
                    SponsorCounties = sponsor.Counties,
                    FloorSponsorId = bill.FloorSponsorId,
                    FloorSponsorName = floorSponsor?.Name,
                    FloorSponsorImageUrl = floorSponsor?.ImageUrl,
                    FloorSponsorParty = floorSponsor?.Party,
                    FloorSponsorReligion = floorSponsor?.Religion,
                    FloorSponsorProfession = floorSponsor?.Profession,
                    FloorSponsorDistrict = floorSponsor?.District,
                    FloorSponsorCounties = floorSponsor?.Counties,
                    GeneralProvisions = bill.GeneralProvisions,
                    HilightedProvisions = bill.HilightedProvisions,
                    LastAction = bill.LastAction,
                    LastActionOwner = bill.LastActionOwner,
                    LastActionTime = bill.LastActionTime,
                    BillUrl = "https://le.utah.gov/~2023/bills/static/" + bill.StateId + ".html",
                    IsTracked = bill.IsTracked,
                    Status = bill.Status,
                    WhenPassed = bill?.WhenPassed,
                    Subjects = ConcatSubjects(bill.Id)
                };
                viewModel.Add(vm);
            }
        }

        if(viewModel == null || !viewModel.Any())
        {
            return null;
        }
        return viewModel;
    }
    private string ConcatSubjects(int id)
    {
        var subjects = _context.Subjects.Where(s => s.BillId == id);
        var retVal = "";
        foreach (var s in subjects)
        {
            retVal += s.Value + ", ";
        }
        if (retVal.Any())
        {
            return retVal.Substring(0, retVal.Length - 2);
        }
        return "";
    }
}
