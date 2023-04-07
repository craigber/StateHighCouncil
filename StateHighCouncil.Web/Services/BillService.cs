using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StateHighCouncil.Web.Data;
using StateHighCouncil.Web.Models;
//using StateHighCouncil.Web.WebUpdater.Data;
//using StateHighCouncil.Web.WebUpdater.Data;

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

    public async Task<BillViewModel> GetBillsAsync(string status, string subject)
    {
        var setting = GetSystemSettings(status, subject);

        var viewModel = new BillViewModel
        {
            SelectedStatus = setting.Status,
            SelectedSubject = setting.Subject,
            ShouldClearAllStatus = "false",
            ShouldClearAllFilters = "false",
            Statuses = await LoadStatusesAsync(setting.Status),
            Subjects = await LoadSubjectsAsync(setting.Subject),
            Bills = await LoadBillsAsync(setting.Status, setting.Subject)
        };
        
        return viewModel;
    }

    public async Task<BillViewModel> UpdateBillAsync(BillUpdateViewModel model) 
    {
        var selectedStatus = string.IsNullOrEmpty(model.SelectedStatus) ? "All" : model.SelectedStatus;
        var selectedSubject = string.IsNullOrEmpty(model.SelectedSubject) ? "All" : model.SelectedSubject;

        if (model.ShouldClearAllStatus == "true")
        {
            await ClearAllAsync(model.SelectedStatus);
        }
        
        var setting = GetSystemSettings(selectedStatus, selectedSubject);

       
        var viewModel = new BillViewModel
        {
            SelectedStatus = setting.Status,
            SelectedSubject = setting.Subject,
            ShouldClearAllStatus = "false",
            ShouldClearAllFilters = "false",
            Statuses = await LoadStatusesAsync(selectedStatus),
            Subjects = await LoadSubjectsAsync(selectedSubject),
            Bills = await LoadBillsAsync(setting.Status, setting.Subject)
        };

        return viewModel;
    }

    private async Task<int> ClearAllAsync(string status)
    {
        var count = 0;

        if (_selectedSession.IsSelected && _selectedSession.IsCurrent)
        {
            List<Bill> bills;

            if (status == "New" || status == "Updated")
            {
                bills = await _context.Bills
                    .Where(b => b.Session == _selectedSession.StateId
                        && b.Status == status)
                    .ToListAsync();
            }
            else if (status == "Tracked")
            {
                bills = await _context.Bills
                    .Where(b => b.Session == _selectedSession.StateId
                         && b.IsTracked)
                    .ToListAsync();
            }
            else
            {
                bills = await _context.Bills
                    .Where(b => b.Session == _selectedSession.StateId)
                    .ToListAsync();
            }
            foreach(var bill in bills)
            {
                bill.Status = "";
                _context.Bills.Update(bill);
            }
            count = await _context.SaveChangesAsync();
        }

        return count;
    }

    private SystemSetting GetSystemSettings(string status, string subject)
    {
        var setting = (_context.SystemSettings).FirstOrDefault();
        var shouldUpdate = false;

        if (setting.Status != status)
        {
            setting.Status = status;
            shouldUpdate = true;
        }
        if (setting.Subject != subject) 
        {
            setting.Subject = subject;
            shouldUpdate = true;
        }

        if (shouldUpdate)
        {
            _context.SystemSettings.Update(setting);
            _context.SaveChanges();
        }
        return setting;
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

    private async Task<SelectList> LoadStatusesAsync(string selectedStatus)
    {
        SelectList selectList = null;
        
        await Task.Run(() =>
        {
            var statuses = new List<SelectItem>();

            statuses.Add(new SelectItem { Id = "All", Name = "All", Selected = ("All" == selectedStatus) });
            statuses.Add(new SelectItem { Id = "New", Name = "New", Selected = ("New" == selectedStatus) });
            statuses.Add(new SelectItem { Id = "Tracked", Name = "Tracked", Selected = ("Tracked" == selectedStatus) });
            statuses.Add(new SelectItem { Id = "Updated", Name = "Updated", Selected = ("Updated" == selectedStatus) });
        
            selectList = new SelectList(statuses, "Id", "Name", "Selected");

            var listItem = selectList
                .FirstOrDefault(s => s.Value == selectedStatus);
            listItem.Selected = true;
        });
        return selectList;
    }

    private async Task<SelectList> LoadSubjectsAsync(string selectedSubject)
    {
        var subjects = await _context.Subjects
            .Where(s => s.SessionStateId == _selectedSession.StateId)
            .GroupBy(g => g.Value)
            .Select(x => new SelectItem
            {
                Name = x.Key,
                Id = x.Key,
                Selected = (x.Key == selectedSubject)
            })
            .OrderBy(o => o.Name)
            .ToListAsync();

        var selectItems = new List<SelectItem>();
        selectItems.Add(new SelectItem { Name = "All", Id = "All", Selected = ("All" == selectedSubject) });
        foreach(var item in subjects)
        {
            selectItems.Add(item);
        }

        var selectList = new SelectList(selectItems, "Id", "Name", "Selected");

        var searchValue = selectedSubject == "All" ? "All" : selectedSubject;
        var listItem = selectList
                .FirstOrDefault(s => s.Value == searchValue);
        listItem.Selected = true;

        return selectList;
    }

    private async Task<List<BillItemViewModel>> LoadBillsAsync(string status, string subject)
    {
        List<Bill> bills;

        if (status == "All")
        {
            bills = await _context.Bills
                .Where(b => b.Session == _selectedSession.StateId)
                .Include(b => b.Subjects)
                .OrderBy(o => o.Version)
                .ToListAsync();
        }
        else if (status == "Tracked")
        {
            bills = await GetTrackedBillsAsync();
        }
        else
        {
            bills = await _context.Bills
                .Where(b => b.Session == _selectedSession.StateId
                    && b.Status == status)
                .Include(b => b.Subjects)
                .OrderBy(o => o.Version)
                .ToListAsync();
        }

        var legislators = await _context.Legislators
            .Where(l => l.Session == _selectedSession.StateId)
            .ToListAsync();

        var billItems = new List<BillItemViewModel>();

        await Task.Run(() =>
        {
            foreach (var bill in bills)
            {
                if (string.IsNullOrEmpty(subject)
                    || subject == "All"
                    || (bill.Subjects != null 
                        && bill.Subjects.Any(s => s.Value == subject)))
                {
                    var sponsor = legislators
                        .FirstOrDefault(l => l.Id == bill.SponsorId);

                    Legislator floorSponsor = null;

                    if (bill.FloorSponsorId > 0)
                    {
                        floorSponsor = legislators
                            .FirstOrDefault(l => l.Id == bill.FloorSponsorId);
                    }

                    var vm = new BillItemViewModel
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
                        Subjects = ConcatSubjects(bill.Id),
                        PlusMinus = bill.PlusMinus,
                        Commentary = bill.Commentary
                    };
                    billItems.Add(vm);
                }
            }
        });

        return billItems;
    }

    private async Task<List<Bill>> GetTrackedBillsAsync()
    {
        var bills = await _context.Bills
            .Where(b => b.Session == _selectedSession.StateId
                && b.IsTracked)
            .Include(b => b.Subjects)
            .OrderBy(o => o.Version)
            .ToListAsync();

        return bills;
    }
}
