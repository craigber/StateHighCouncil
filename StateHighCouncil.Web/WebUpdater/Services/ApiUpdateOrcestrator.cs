using Newtonsoft.Json;
using ApiLegislators = StateHighCouncil.WebDataUpdater.Data.Legislators;
using LocalData = StateHighCouncil.Web.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StateHighCouncil.WebDataUpdater.Data.Legislators;
using Microsoft.EntityFrameworkCore;
using StateHighCouncil.Web.Data;
using StateHighCouncil.Web.Data.Models;
using SQLitePCL;
using StateHighCouncil.Web.WebUpdater.Services;

namespace StateHighCouncil.Web.WebUpdater.Services;

public class ApiUpdateOrchestrator : IUpdateOrchestrator
{
    private readonly DataContext _context;
    private readonly IApiUpdater _rosterUpdater;
    private readonly IBillsUpdater _billsUpdater;

    public ApiUpdateOrchestrator(DataContext context,
        IApiUpdater rosterUpdater,
        IBillsUpdater billsUpdater)
    {
        _context = context;
        _rosterUpdater = rosterUpdater;
        _billsUpdater = billsUpdater;
    }

    public async Task<int> UpdateAsync()
    {
        await _rosterUpdater.UpdateAsync();
        await _billsUpdater.UpdateAsync();
        return 1;
    }


    //public async Task<int> UpdateAsync()
    //{
     
    //    LegislatorsApiRoot? rootobject;

    //    var httpClient = new HttpClient
    //    {
    //        BaseAddress = new Uri("https://glen.le.utah.gov/")
    //    };

    //    var temp = _context.Legislators.ToList();

    //    HttpResponseMessage? response = await httpClient.GetAsync("legislators/FEC811BB64D504504F76C92DE73FA261");
        
    //    var apiResponse = await response.Content.ReadAsStringAsync();
    //    rootobject = JsonConvert.DeserializeObject<LegislatorsApiRoot>(apiResponse);
                
    //    if (rootobject == null)
    //    {
    //        return 0;
    //    }
        
    //    foreach (var apiLeg in rootobject.legislators)
    //    {
    //        var legislators = _context.Legislators
    //            .Where(l => l.StateId == apiLeg.id
    //            && l.District.ToString() == apiLeg.district
    //            && l.House == apiLeg.house).ToList();

    //        var legislator = new StateHighCouncil.Web.Data.Models.Legislator();

    //        if (legislators.Any())
    //        {
    //            legislator = legislators.FirstOrDefault();
    //        }

    //        legislator.Address = apiLeg.address;
    //        legislator.Bio = apiLeg.bio;
    //        legislator.Cell = apiLeg.cell;
    //        legislator.Counties = apiLeg.counties;
    //        legislator.DemographicUrl = apiLeg.demographic;
    //        legislator.District = int.Parse(apiLeg.district);
    //        legislator.Education = apiLeg.education;
    //        legislator.Email = apiLeg.email;
    //        legislator.FacebookUrl = apiLeg.facebook;
    //        legislator.Fax = apiLeg.fax;
    //        legislator.HomePhone = apiLeg.homePhone;
    //        legislator.House = apiLeg.house;
    //        legislator.ImageUrl = apiLeg.image;
    //        legislator.InstagramUrl = apiLeg.instagram;
    //        legislator.LegislationUrl = apiLeg.legislation;
    //        legislator.Name = apiLeg.formatName;
    //        legislator.Party = apiLeg.party;
    //        legislator.Position = apiLeg.position;
    //        legislator.Profession = apiLeg.profession;
    //        legislator.ProfessionalAffiliations = apiLeg.professionalAffiliations;
    //        legislator.RecognitionsAndHonors = apiLeg.recognitionsAndHonors;
    //        legislator.ServiceStart = apiLeg.serviceStart;
    //        legislator.StateId = apiLeg.id;
    //        legislator.TwitterUrl = apiLeg.twitter;
    //        legislator.WorkPhone = apiLeg.workPhone;
    //        legislator.WhenLastUpdated = DateTime.Now;
    //        legislator.Religion = CalculateReligion(apiLeg);

    //        if (legislator.Id <= 0)
    //        {
    //            legislator.Status = "New";
    //            legislator.WhenAdded = DateTime.Now;
    //        }
    //        _context.Legislators.Add(legislator);
    //        _context.SaveChanges();

    //        // Now handle committee assignments
    //        if (apiLeg.committees.Any())
    //        {
    //            foreach (var comm in apiLeg.committees)
    //            {
    //                var committee = _context.Committees
    //                    .Where(c => c.StateId == comm.committee).FirstOrDefault();
    //                if (committee == null)
    //                {
    //                    committee = new StateHighCouncil.Web.Data.Models.Committee();
    //                    committee.StateId = comm.committee;
    //                    committee.Name = comm.name;
    //                    _context.Committees.Add(committee);
    //                    _context.SaveChanges();
    //                }

    //                var assignment = _context.CommitteeAssignments
    //                    .Where(a => a.Id == committee.Id
    //                    && a.LegislatorId == legislator.Id).FirstOrDefault();
    //                if (assignment == null)
    //                {
    //                    assignment = new CommitteeAssignment
    //                    {
    //                        CommitteeId = committee.Id,
    //                        LegislatorId = legislator.Id
    //                    };
    //                    _context.CommitteeAssignments.Add(assignment);
    //                    _context.SaveChanges();
    //                }
    //            }

    //        }
            
    //    }
    //    return 1;
    //}

    //private string CalculateReligion(ApiLegislators.Legislator person)
    //{
    //    if (!string.IsNullOrEmpty(person.bio))
    //    {
    //        var bio = person.bio.ToLower();

    //        if (bio.Contains("byu")
    //        || bio.Contains("brigham young university")
    //        || bio.Contains("latter day")
    //        || bio.Contains("latter-day"))
    //        {
    //            return "LDS";
    //        }
    //    }

    //    if (!string.IsNullOrEmpty(person.education))
    //    {
    //        var education = person.education.ToLower();

    //        if (education.Contains("byu")
    //        || education.Contains("brigham young university")
    //        || education.Contains("latter day")
    //        || education.Contains("latter-day"))
    //        {
    //            return "LDS";
    //        }
    //    }

    //    if (!string.IsNullOrEmpty(person.professionalAffiliations))
    //    {
    //        var affiliations = person.professionalAffiliations.ToLower();

    //        if (affiliations.Contains("byu")
    //        || affiliations.Contains("brigham young university")
    //        || affiliations.Contains("latter day")
    //        || affiliations.Contains("latter-day"))
    //        {
    //            return "LDS";
    //        }
    //    }
    //    return "";

    //}
}