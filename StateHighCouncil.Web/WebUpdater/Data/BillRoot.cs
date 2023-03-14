namespace StateHighCouncil.Web.WebUpdater.Data;

public class BillRootobject
{
    public string bill { get; set; }  // StateId
    public string version { get; set; }
    public string shorttitle { get; set; }
    public string sponsor { get; set; }
    public string floorsponsor { get; set; }
    public string generalprovisions { get; set; }
    public string hilightedprovisions { get; set; }
    public string monies { get; set; }
    public string attorney { get; set; }
    public string fiscalanalyst { get; set; }
    public string lastaction { get; set; }
    public string lastactionowner { get; set; }
    public string lastactiontime { get; set; }
    public string trackingid { get; set; }
    public string[] subjects { get; set; }
    public string[] codesections { get; set; }
    public object[] agendas { get; set; }
    public ActionItem[] actionhistory { get; set; }
}
