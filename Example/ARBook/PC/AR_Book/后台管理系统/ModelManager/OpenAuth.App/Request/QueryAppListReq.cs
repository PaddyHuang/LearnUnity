namespace OpenAuth.App.Request
{
    public class QueryAppListReq : PageReq
    {
        public string MenuID { get; set; }

        public int Type { get; set; }
    }
}
