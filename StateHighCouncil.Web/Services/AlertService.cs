using StateHighCouncil.Web.Data;
using System.Text;

namespace StateHighCouncil.Web.Services;

public class AlertService : IAlertService
{
    private readonly DataContext _context;
    public AlertService(DataContext context)
    {
        _context = context;
    }

    public string GetSessionMessage()
    {
        var currentSession = _context.Sessions.FirstOrDefault(s => s.IsCurrent);

        // Current Session
        if (currentSession.WhenBegin >= DateTime.Now
            && currentSession.WhenEnd <= DateTime.Now) 
        {
            var text = "The " + currentSession.Name + " is currently running until "
                + currentSession.WhenEnd.ToString("MMMM dd, yyyy");
            return FormatAlert(text, "success");
        }

        // New Session is upcoming
        if (currentSession.WhenBegin >= new DateTime(DateTime.Now.Year, 1, 1))
        {
            var text = "The " + currentSession.Name + " will begin on "
                + currentSession.WhenBegin.ToString("MMMM dd, yyyy");
            return FormatAlert(text, "danger");
        }

        return "";
    }

    private string FormatAlert(string text, string alertType)
    {
        var alert = new StringBuilder();
        alert.Append("<div class='mx=2 alert alert-");
        alert.Append(alertType);
        alert.Append("' role='alert'>");
        alert.Append(text);
        alert.Append("</div>");
        return alert.ToString();
    }
}

