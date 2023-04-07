using Microsoft.EntityFrameworkCore;
using StateHighCouncil.Web.Controllers;
using StateHighCouncil.Web.Data;
using StateHighCouncil.Web.Models;

namespace StateHighCouncil.Web.Services
{
    public class LegislatorService : ILegislatorService
    {
        private readonly DataContext _context;
        private readonly Session _selectedSession;

        public LegislatorService(DataContext context)
        {
            _context = context;
            _selectedSession = _context.Sessions.Where(s => s.IsSelected).FirstOrDefault();
        }

        public async Task<List<LegislatorListViewModel>> GetLegislatorsListAsync(string house)
        {
            if (house == "H" || house == "S")
            {
                var legislators = _context.Legislators
                    .Where(l => l.House == house && l.Session == _selectedSession.StateId)
                    .OrderBy(l => l.District);

                var bills = _context.Bills
                    .Where(s => s.Session == _selectedSession.StateId);

                var viewModel = new List<LegislatorListViewModel>();
                foreach (var l in legislators)
                {
                    var legBills = bills.Where(b => b.SponsorId == l.Id
                        || b.FloorSponsorId == l.Id);

                    var newLegsilator = new LegislatorListViewModel
                    {
                        Id = l.Id,
                        Name = l.Name,
                        ImageUrl = l.ImageUrl,
                        StateId = l.StateId,
                        ServiceStart = l.ServiceStart,
                        Position = l.Position,
                        Counties = l.Counties,
                        District = l.District,
                        Profession = l.Profession,
                        Industry = l.Industry,
                        Status = l.Status,
                        Party = l.Party,
                        Religion = l.Religion,
                        Education = l.Education,
                        SponsorFiledCount = legBills
                            .Count(b => b.SponsorId == l.Id),
                        SponsorPassedCount = legBills
                            .Count(b => b.SponsorId == l.Id && b.WhenPassed > new DateTime(1, 1, 1)),
                        FloorSponsorFiledCount = legBills
                            .Count(b => b.FloorSponsorId == l.Id),
                        FloorSponsorPassedCount = legBills
                            .Count(b => b.FloorSponsorId == l.Id && b.WhenPassed > new DateTime(1, 1, 1))
                    };
                    viewModel.Add(newLegsilator);
                }

                return viewModel;
            }
            return null;
        }

        public async Task<LegislatorViewModel> GetLegislatorAsync(int id)
        {
            if (id <= 0)
            {
                return null;            }

            var legislator = await _context.Legislators
                .Include(c => c.Committees)
                .Include(f => f.FinanceReports)
                .Include(ci => ci.Conflicts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (legislator == null)
            {
                return null;
            }

            var viewModel = MapLegislator(legislator);
            return viewModel;
        }

        private LegislatorViewModel MapLegislator(Legislator legislator)
        {
            var vm = new LegislatorViewModel
            {
                Id = legislator.Id,
                Name = legislator.Name,
                StateId = legislator.StateId,
                ImageUrl = legislator.ImageUrl,
                House = legislator.House,
                Party = legislator.Party,
                Position = legislator.Position,
                District = legislator.District,
                ServiceStart = legislator.ServiceStart,
                Profession = legislator.Profession,
                Industry = legislator.Industry,
                ProfessionalAffiliations = legislator.ProfessionalAffiliations,
                Education = legislator.Education,
                RecognitionsAndHonors = legislator?.RecognitionsAndHonors,
                Counties = legislator?.Counties,
                Address = legislator?.Address,
                Email = legislator?.Email,
                Cell = legislator?.Cell,
                WorkPhone = legislator?.WorkPhone,
                Committees = GetCommittees(legislator.Committees),
                LegislationUrl = legislator?.LegislationUrl,
                DemographicUrl = legislator?.DemographicUrl,
                FinanceReports = legislator.FinanceReports,
                Religion = legislator?.Religion,
                Gender = legislator?.Gender,
                Status = legislator?.Status,
                TwitterUrl = legislator?.TwitterUrl,
                FacebookUrl = legislator?.FacebookUrl,
                HomePhone = legislator?.HomePhone,
                Bio = legislator?.Bio,
                Fax = legislator?.Fax,
                InstagramUrl = legislator?.InstagramUrl,
                WhenAdded = legislator.WhenAdded,
                WhenLastUpdated = legislator.WhenLastUpdated,
                Conflicts = legislator.Conflicts,
                //Sessions = legislator.Sessions,
                Bills = GetBills(legislator.Id)
            };

            return vm;
        }

        private List<LegislatorBillViewModel> GetBills(int legislatorId)
        {
            var sponsoredBills = new List<LegislatorBillViewModel>();

            var bills = _context.Bills
                .Where(l => l.SponsorId == legislatorId || l.FloorSponsorId == legislatorId)
                .OrderBy(o => o.StateId);

            if (bills.Any())
            {
                foreach (Models.Bill bill in bills)
                {
                    var sponsored = new LegislatorBillViewModel
                    {
                        Id = bill.Id,
                        Number = bill.Version,
                        Title = bill.ShortTitle,
                        LastAction = bill.LastAction,
                        LastActionTime = bill.LastActionTime,
                        Status = bill.Status,
                        IsTracked = bill.IsTracked,
                        Sponsor = CalculateSponsor(legislatorId, bill),
                        WhenPassed = bill.WhenPassed,
                        GeneralProvisions = bill.GeneralProvisions,
                        BillUrl = "https://le.utah.gov/~2023/bills/static/" + bill.StateId + ".html"
                    };
                    sponsoredBills.Add(sponsored);
                }
            }
            return sponsoredBills;
        }

        private string CalculateSponsor(int legislatorId, Models.Bill bill)
        {
            if (bill.SponsorId == legislatorId)
            {
                return "Sponsor";
            }
            if (bill.FloorSponsorId == legislatorId)
            {
                return "Floor Sponsor";
            }
            return "";

        }

        private List<Committee> GetCommittees(List<CommitteeAssignment> assignments)
        {
            var legCommittees = new List<Committee>();

            if (assignments != null && assignments.Any())
            {
                var committees = _context.Committees.ToList();

                foreach (CommitteeAssignment assignment in assignments)
                {
                    legCommittees.Add(committees
                        .FirstOrDefault(c => c.Id == assignment.CommitteeId));
                }
            }

            return legCommittees;
        }
    }
}
