using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StateHighCouncil.Web.Data;
using StateHighCouncil.Web.Models;
using StateHighCouncil.WebDataUpdater.Data.Legislators;
using ApiLegislators = StateHighCouncil.WebDataUpdater.Data.Legislators;

namespace StateHighCouncil.Web.WebUpdater.Services
{
    public class RosterApiUpdater : IApiUpdater
    {
        private readonly DataContext _context;
        private LegislatorsApiRoot? _rootObject;
        private HttpClient _httpClient;
        private ReligionCalculator _religionCalculator;
        private int _currentSessionId;

        public RosterApiUpdater(DataContext context)
        {
            _context = context;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://glen.le.utah.gov/")
            };
        }

        public async Task<int> UpdateAsync()
        {
            var temp = _context.Legislators.ToList();

            HttpResponseMessage? response = await _httpClient.GetAsync("legislators/FEC811BB64D504504F76C92DE73FA261");

            var apiResponse = await response.Content.ReadAsStringAsync();
            _rootObject = JsonConvert.DeserializeObject<LegislatorsApiRoot>(apiResponse);

            if (_rootObject == null)
            {
                return 0;
            }

            foreach (var apiLeg in _rootObject.legislators)
            {
                UpdateSingleLegislator(apiLeg);
            }
            return 1;
        }

        private void UpdateSingleLegislator(ApiLegislators.Legislator apiLeg)
        {
            _religionCalculator = new ReligionCalculator();

            _currentSessionId = _context.Sessions.Where(s => s.IsCurrent).FirstOrDefault().Id;

            var legislators = _context.Legislators
                    .Where(l => l.StateId == apiLeg.id
                    && l.District.ToString() == apiLeg.district
                    && l.House == apiLeg.house).ToList();

            var legislator = new Models.Legislator();

            if (legislators.Any())
            {
                legislator = legislators.FirstOrDefault();
            }

            legislator.Address = apiLeg.address;
            legislator.Bio = apiLeg.bio;
            legislator.Cell = apiLeg.cell;
            legislator.Counties = apiLeg.counties;
            legislator.DemographicUrl = apiLeg.demographic;
            legislator.District = int.Parse(apiLeg.district);
            legislator.Education = apiLeg.education;
            legislator.Email = apiLeg.email;
            legislator.FacebookUrl = apiLeg.facebook;
            legislator.Fax = apiLeg.fax;
            legislator.HomePhone = apiLeg.homePhone;
            legislator.House = apiLeg.house;
            legislator.ImageUrl = apiLeg.image;
            legislator.InstagramUrl = apiLeg.instagram;
            legislator.LegislationUrl = apiLeg.legislation;
            legislator.Name = apiLeg.formatName;
            legislator.Party = apiLeg.party;
            legislator.Position = apiLeg.position;
            legislator.Profession = apiLeg.profession;
            legislator.ProfessionalAffiliations = apiLeg.professionalAffiliations;
            legislator.RecognitionsAndHonors = apiLeg.recognitionsAndHonors;
            legislator.ServiceStart = apiLeg.serviceStart;
            legislator.StateId = apiLeg.id;
            legislator.TwitterUrl = apiLeg.twitter;
            legislator.WorkPhone = apiLeg.workPhone;
            legislator.WhenLastUpdated = DateTime.Now;
            legislator.Religion = _religionCalculator.Calculate(apiLeg);

            if (legislator.Id <= 0)
            {
                legislator.Status = "New";
                legislator.WhenAdded = DateTime.Now;
            }
            _context.Legislators.Add(legislator);
            _context.SaveChanges();

            // Update session assignment
            var sessionAssignment = new SessionAssignment
            {
                LegislatorId = legislator.Id,
                SessionId = _currentSessionId
            };
            _context.SessionAssignments.Add(sessionAssignment);
            _context.SaveChanges();

            // Conflicts of Interest
            if (apiLeg.CofI != null && apiLeg.CofI.Any())
            {
                var savedConflicts = _context.Conflicts
                    .Where(c => c.LegislatorId == legislator.Id)
                    .ToList();
                if(!savedConflicts.Any())
                {
                    savedConflicts = new List<Conflict>();
                    foreach(var conflict in apiLeg.CofI)
                    {
                        if(!savedConflicts.Exists(c => c.ConflictUrl == conflict.url))
                        {
                            var newConflict = new Conflict
                            {
                                LegislatorId = legislator.Id,
                                ConflictUrl = conflict.url
                            };
                            _context.Conflicts.Add(newConflict);
                            _context.SaveChanges();
                        }
                    }
                }
            }

            // Finance Reports
            if (apiLeg.FinanceReport != null && apiLeg.FinanceReport.Any())
            {
                var savedReports = _context.FinanceReports
                    .Where(f => f.LegislatorId == legislator.Id)
                    .ToList();
                if(!savedReports.Any())
                {
                    savedReports = new List<FinanceReport>();
                }

                foreach(var report in apiLeg.FinanceReport)
                {
                    if(!savedReports.Exists(r => r.FinanceReportUrl == report.url))
                    {
                        var newReport = new FinanceReport
                        {
                            LegislatorId = legislator.Id,
                            FinanceReportUrl = report.url
                        };
                        _context.FinanceReports.Add(newReport);
                        _context.SaveChanges();
                    }                    
                }
            }
             
            // Now handle committee assignments
            if (apiLeg.committees.Any())
            {
                foreach (var comm in apiLeg.committees)
                {
                    var committee = _context.Committees
                        .Where(c => c.StateId == comm.committee).FirstOrDefault();
                    if (committee == null)
                    {
                        committee = new StateHighCouncil.Web.Models.Committee();
                        committee.StateId = comm.committee;
                        committee.Name = comm.name;
                        _context.Committees.Add(committee);
                        _context.SaveChanges();
                    }

                    var assignment = _context.CommitteeAssignments
                        .Where(a => a.Id == committee.Id
                        && a.LegislatorId == legislator.Id).FirstOrDefault();
                    if (assignment == null)
                    {
                        assignment = new CommitteeAssignment
                        {
                            CommitteeId = committee.Id,
                            LegislatorId = legislator.Id
                        };
                        _context.CommitteeAssignments.Add(assignment);
                        _context.SaveChanges();
                    }
                }

            }
        }        
    }
}
