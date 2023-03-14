namespace StateHighCouncil.Web.WebUpdater.Data
{

    public class BillListRootobject
    {
        public Bill[] bills { get; set; }
    }

    public class Bill
    {
        public string number { get; set; }
        public string updatetime { get; set; }
    }

}
