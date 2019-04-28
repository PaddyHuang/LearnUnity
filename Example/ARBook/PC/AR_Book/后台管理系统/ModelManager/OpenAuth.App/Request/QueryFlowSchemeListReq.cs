namespace OpenAuth.App.Request
{
    public class QueryFlowSchemeListReq : PageReq
    {
        public string orgId { get; set; }

        public int TypeCode { get; set; }
    }
}
