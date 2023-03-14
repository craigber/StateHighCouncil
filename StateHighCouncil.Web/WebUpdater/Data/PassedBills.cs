namespace StateHighCouncil.Web.WebUpdater.Data;

public class PassedBillsRootobject
{
    public Passedbill[] passedbills { get; set; }
}

public class Passedbill
{
    public string number { get; set; }
    public string title { get; set; }
    public string sponsor { get; set; }
    public string datepassed { get; set; }
    public string effectivedate { get; set; }
    public string govaction { get; set; }
    public string govactiondate { get; set; }
    public string chapter { get; set; }
}
