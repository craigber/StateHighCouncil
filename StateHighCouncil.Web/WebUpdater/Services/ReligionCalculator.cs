using StateHighCouncil.WebDataUpdater.Data.Legislators;

namespace StateHighCouncil.Web.WebUpdater.Services;

public class ReligionCalculator
{
    public string Calculate(Legislator person)
    {
        if (IsLds(person.bio)
            || IsLds(person.education)
            || IsLds(person.professionalAffiliations))
        {
            return "LDS";
        }

        return "";
    }

    private bool IsLds(string testValue)
    {
        if (string.IsNullOrEmpty(testValue)) return false;

        var value = testValue.ToLower();

        return (value.Contains("byu")
            || value.Contains("brigham young university")
            || value.Contains("latter day")
            || value.Contains("latter-day"));            
    }
}
