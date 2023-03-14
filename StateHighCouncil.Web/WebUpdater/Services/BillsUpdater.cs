using Newtonsoft.Json;
using StateHighCouncil.Web.Data;
using StateHighCouncil.WebDataUpdater.Data.Legislators;
using StateHighCouncil.Web.WebUpdater.Data;
using StateHighCouncil.Web.Models;
using static System.Net.WebRequestMethods;

namespace StateHighCouncil.Web.WebUpdater.Services;

public class BillsUpdater : IBillsUpdater
{
    private readonly DataContext _context;
    private BillListRootobject? _listRoot;
    private HttpClient _httpClient;
    private int _currentSessionId;
    private List<Models.Legislator> _legislators;
    private PassedBillsRootobject _passedBills;

    public BillsUpdater(DataContext context)
    {
        _context = context;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://glen.le.utah.gov/")
        };
    }

    public async Task<int> UpdateAsync()
    {
        var currentSession = _context.Sessions
            .Where(s => s.IsCurrent).SingleOrDefault();

        if (currentSession == null) { return 0; }

        var queryPath = "/bills/"
            + currentSession.StateId
            + "/billlist/FEC811BB64D504504F76C92DE73FA261";

        // Get the list of bills
        HttpResponseMessage? response = await _httpClient.GetAsync(queryPath);

        var apiResponse = await response.Content.ReadAsStringAsync();
        _listRoot = JsonConvert.DeserializeObject<BillListRootobject>(apiResponse);

        if (_listRoot == null)
        {
            return 0;
        }

        // Get the list of passed bills
        queryPath = "/bills/" + currentSession.StateId + "/passedlist/"
            + "FEC811BB64D504504F76C92DE73FA261";
        response = await _httpClient.GetAsync(queryPath);
        apiResponse = await response.Content.ReadAsStringAsync();
        _passedBills = JsonConvert.DeserializeObject<PassedBillsRootobject>(apiResponse);

        var l = _context.Legislators;
        _legislators = _context.Legislators.ToList();

        // Get each bill
        foreach (var bl in _listRoot.bills)
        {
            // Find the local copy of the bill
            var localBill = _context.Bills
                .Where(b => b.StateId == bl.number)
                .FirstOrDefault();
            DateTime whenSaved = DateTime.Now;
            whenSaved = ParseDateTime(bl.updatetime);
            

            if (localBill == null)
            {
                localBill = new Models.Bill();
            }
            

            if (localBill == null || localBill.LastActionTime < whenSaved)
            {
                // It's either new or updated so get the bill
                queryPath = "/bills/" + currentSession.StateId + "/" 
                    + bl.number + "/FEC811BB64D504504F76C92DE73FA261";

                response = await _httpClient.GetAsync(queryPath);
                apiResponse = await response.Content.ReadAsStringAsync();
                var billRoot = JsonConvert.DeserializeObject<BillRootobject>(apiResponse);

                

                localBill.Attorney = billRoot.attorney;
                localBill.SponsorId = GetLegislatorId(billRoot.sponsor);
                localBill.SponsorStateId = billRoot.sponsor;
                localBill.Version = billRoot.version;
                localBill.FiscalAnalyst = billRoot.fiscalanalyst;
                localBill.FloorSponsorId = GetLegislatorId(billRoot.floorsponsor);
                localBill.FloorSponsorStateId = billRoot.floorsponsor;
                localBill.GeneralProvisions = billRoot.generalprovisions;
                localBill.HilightedProvisions = billRoot.hilightedprovisions;
                localBill.LastAction = billRoot.lastaction;
                localBill.LastActionOwner = billRoot.lastactionowner;
                localBill.LastActionTime = ParseDateTime(billRoot.lastactiontime);
                localBill.Monies = billRoot.monies;
                localBill.ShortTitle = billRoot.shorttitle;
                localBill.StateId = billRoot.bill;
                localBill.TrackingId = billRoot.trackingid;
                localBill.WhenPassed = GetPassedDate(billRoot.version);
                
                if(localBill.Id < 1)
                {
                    localBill.Status = "New";
                    localBill.WhenCreated = DateTime.Now;
                    localBill.WhenLastUpdated = DateTime.Now;
                    _context.Bills.Add(localBill);
                }
                else
                {
                    localBill.WhenLastUpdated = DateTime.Now;
                    localBill.Status = "Updated";
                    _context.Bills.Update(localBill);
                }
                _context.SaveChanges();

                // Save subjects
                if (billRoot.subjects != null && billRoot.subjects.Any())
                {
                    var localSubjects = _context.Subjects.Where(s => s.Id == localBill.Id);

                    foreach (var subject in billRoot.subjects)
                    {
                        if (!localSubjects.Any(l => l.Value == subject))
                        {
                            var newSubject = new Models.Subject
                            {
                                BillId = localBill.Id,
                                Value = subject
                            };

                            _context.Subjects.Add(newSubject);
                            _context.SaveChanges();
                        }
                    }                    
                }

                // Save ActionHistory
                //if (billRoot.actionhistory != null && billRoot.actionhistory.Any())
                //{
                //    var localActions = _context.ActionHistories
                //        .Where(a => a.BillId == localBill.Id);

                //    foreach (ActionItem action in billRoot.actionhistory)
                //    {
                //        if (!(localActions.Any(l => l.Date == action.Date
                //            && l.Action == action.Action 
                //            && l.Location == action.Action)))
                //        { 
                //            var newAction = new Models.ActionHistory
                //            {
                //                BillId = localBill.Id,
                //                Date = action.Date,
                //                Action = action.Action,
                //                Location = action.Location
                //            };

                //            _context.ActionHistories.Add(newAction);
                //            _context.SaveChanges();
                //        }
                //    }
                //}


            }
        }

        return 0;
    }

    private DateTime GetPassedDate(string billNumber)
    {
        if (string.IsNullOrEmpty(billNumber))
        { 
            return new DateTime();
        }

        var passed = _passedBills.passedbills
            .FirstOrDefault(p => p.number == billNumber);

        if (passed == null)
        {
            return new DateTime();
        }
        return ParseDateTime(passed.datepassed);      
    }

    private int GetLegislatorId(string stateId)
    {
        if(string.IsNullOrEmpty(stateId))
        { return 0; }

        

        return _legislators.FirstOrDefault(l => l.StateId == stateId).Id;
    }

    private DateTime ParseDateTime(string value)
    {
        var indexZ = value.IndexOf('T');
        if (indexZ >= 0 && value[value.Length - 1] == 'Z')
        {
            if (value[indexZ + 3] != ':')
            {
                value = value.Insert(indexZ + 1, "0");
            }
        }

        return DateTime.Parse(value);
    }
}
