using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StateHighCouncil.Web.Data;
using StateHighCouncil.Web.Models;

namespace StateHighCouncil.Web.Controllers;

public class ApiController : Controller
{
    private readonly DataContext _context;
    private readonly Session _selectedSession;

    public ApiController(DataContext context)
    {
        _context = context;
        _selectedSession = _context.Sessions.Where(s => s.IsSelected).FirstOrDefault();
    }

    [HttpGet]
    public async Task<JsonResult> ClearFilters()
    {
        var setting = (_context.SystemSettings).FirstOrDefault();
        setting.Status = "All";
        setting.Subject = "All";
        _context.SystemSettings.Update(setting);
        await _context.SaveChangesAsync();
        return Json(true);
    }

    [HttpGet]
    public bool ToggleBillTracking(int billId)
    {
        if (billId < 1)
        {
            return false;
        }
        
        var bill = _context.Bills.FirstOrDefault(b => b.Id == billId);
        if (bill != null)
        {
            bill.IsTracked = !bill.IsTracked;
            _context.Bills.Update(bill);
            _context.SaveChanges();
        }
        return bill.IsTracked;
    }

    [HttpGet]
    public JsonResult Subjects()
    {
        var subjects = _context.Subjects.Where(s => s.SessionStateId == _selectedSession.StateId)
            .GroupBy(g => g.Value)
            .Select(x => new
            {
                Name = x.Key
            })
            
            .OrderBy(s => s.Name)
            .ToList();

        return Json(subjects);
    }

    [HttpGet]
    public JsonResult ClearStatus(int billId)
    {
        if (billId < 1)
        { 
            return Json(false);
        }
        
        var bill = _context.Bills.Where(b => b.Id == billId).FirstOrDefault();
        if (bill == null)
        {
            return Json(false);
        }
        
        bill.Status = "";
        _context.Bills.Update(bill);

        if (_context.SaveChanges() == 1)
        {
            return Json(true);
        }
        return Json(false);
    }

    public async Task<JsonResult> ClearLegislatorStatus(int id)
    {
        if (id < 1)
        {
            return Json(false);
        }

        var leg = _context.Legislators.Where(l => l.Id == id).FirstOrDefault();

        if (leg == null)
        {
            return Json(false);
        }

        leg.Status = "";
        _context.Legislators.Update(leg);
        await _context.SaveChangesAsync();
        return Json(true);
    }
}
