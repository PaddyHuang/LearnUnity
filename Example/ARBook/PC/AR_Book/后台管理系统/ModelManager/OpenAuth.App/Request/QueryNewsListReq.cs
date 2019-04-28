using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.App.Request
{


    /// <summary>
    /// 查找资讯列表
    /// </summary>
   public class QueryNewsListReq:PageReq
    {
        public string ProID { get; set; }

        public int TypeValue { get; set; }
      
    }
}
