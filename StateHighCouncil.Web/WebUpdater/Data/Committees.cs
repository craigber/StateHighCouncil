namespace StateHighCouncil.WebDataUpdater.Data.Committees;
public class CommitteesApiRoot
{
    public Committee[] committees { get; set; }
}

public class Committee
{
    public string description { get; set; }
    public string id { get; set; }
    public string link { get; set; }
    public Meeting[] meetings { get; set; }
    public Member[] members { get; set; }
}

public class Meeting
{
    public DateTime mtgTime { get; set; }
    public string mtgPlace { get; set; }
    public string status { get; set; }
}

public class Member
{
    public string id { get; set; }
    public string position { get; set; }
    public string name { get; set; }
}
