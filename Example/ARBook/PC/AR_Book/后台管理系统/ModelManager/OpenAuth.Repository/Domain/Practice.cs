using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Repository.Domain
{
    /// <summary>
    /// 练习题
    /// </summary>
   public class Practice : Entity
    {
        /// <summary>
        /// 练习ID
        /// </summary>
          public string Id{get;set;}

        /// <summary>
        /// 相关 模型ID
        /// </summary>
          public string ModelID{get;set;}
        /// <summary>
        /// 练习题名称
        /// </summary>
          public string Name{get;set;}
        /// <summary>
        /// 练习题内容
        /// </summary>
          public string Content{get;set;}

        /// <summary>
        /// 状态
        /// </summary>
          public int Flag{get;set;}
        /// <summary>
        /// 创建时间
        /// </summary>
          public DateTime CreateTime{get;set;}

        /// <summary>
        /// 更新时间
        /// </summary>
          public DateTime UpdateTime { get; set; }

    }
}
