using Microsoft.AspNetCore.Mvc;
using StateHighCouncil.Web.Data;

namespace StateHighCouncil.Web.Controllers;

public class ApiController : Controller
{
    private readonly DataContext _context;

    public ApiController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public bool ToggleBillTracking(int billId)
    {
        if (billId < 1)
        {
            return false; ;
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

}
