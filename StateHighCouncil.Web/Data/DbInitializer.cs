using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using StateHighCouncil.Web.Models;

namespace StateHighCouncil.Web.Data;

public class DbInitializer
{
    public static void Initialize(DataContext context)
    {
        context.Database.EnsureCreated();
        SeedSessions(context);
        SeedLegislators(context);
        SeedSessionAssignments(context);
    }

    private static void SeedSessions(DataContext context) 
    {
        if (context.Sessions.Any())
        {
            return;
        }
        var session = new Session
        {
            Name = "2023 General Session",
            StateId = "2023GS",
            IsCurrent = true
        };
        context.Sessions.Add(session);
        context.SaveChanges();
    }

    private static void SeedLegislators(DataContext context)
    {
        if (context.Legislators.Any())
        {
            return;
        }

        var leg = new Legislator
        {
            StateId = "CRAIGBER",
            District = 0,
            House = "X"
        };

        context.Legislators.Add(leg);
        context.SaveChanges();
    }

    private static void SeedSessionAssignments(DataContext context)
    {
        if (context.SessionAssignments.Any())
        {
            return;
        }

        var sessionAssignment = new SessionAssignment
        {
            LegislatorId = 1,
            SessionId = 1
        };

        context.SessionAssignments.Add(sessionAssignment);
        context.SaveChanges();
    }
}
