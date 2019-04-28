using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Repository.Domain
{
  public class ComIntroduce
    {

        /// <summary>
        /// 唯一标志
        /// </summary>
        public string Id { get ;set; }


        /// <summary>
        /// 公司详细内容
        /// </summary>
        public string TContent { get;set ; }


        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
 
        /// <summary>
        ///状态
        /// </summary>
        public int Flag { get; set; }


        /// <summary>
        /// 类型
        /// </summary>
        public int Cate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>

        public DateTime CreateTime { get; set; }


        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get;set; }
    


    }
}
