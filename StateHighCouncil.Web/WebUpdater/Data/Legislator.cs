namespace StateHighCouncil.WebDataUpdater.Data.Legislators;

public class LegislatorsApiRoot
{
    public Legislator[] legislators { get; set; }
}

public class Legislator
{
    public string fullName { get; set; }
    public string formatName { get; set; }
    public string id { get; set; }
    public string image { get; set; }
    public string house { get; set; }
    public string party { get; set; }
    public string position { get; set; }
    public string district { get; set; }
    public string serviceStart { get; set; }
    public string profession { get; set; }
    public string professionalAffiliations { get; set; }
    public string education { get; set; }
    public string recognitionsAndHonors { get; set; }
    public string counties { get; set; }
    public string address { get; set; }
    public string email { get; set; }
    public string cell { get; set; }
    public string workPhone { get; set; }
    public Committee[] committees { get; set; }
    public string legislation { get; set; }
    public string demographic { get; set; }
    public Financereport[] FinanceReport { get; set; }
    public Cofi[] CofI { get; set; }
    public string twitter { get; set; }
    public string facebook { get; set; }
    public string homePhone { get; set; }
    public string bio { get; set; }
    public string fax { get; set; }
    public string instagram { get; set; }
    public Housedistrict[] houseDistricts { get; set; }
}

public class Committee
{
    public string committee { get; set; }
    public string name { get; set; }
}

public class Financereport
{
    public string url { get; set; }
}

public class Cofi
{
    public string url { get; set; }
}

public class Housedistrict
{
    public string dist { get; set; }
    public string rep { get; set; }
    public string link { get; set; }
}
