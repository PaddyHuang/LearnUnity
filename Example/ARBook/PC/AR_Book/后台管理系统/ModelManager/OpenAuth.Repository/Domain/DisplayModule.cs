using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Repository.Domain
{
  public class DisplayModule:Entity
    {

        /// <summary>
        /// 唯一标志ID
        /// </summary>
          public new string Id{get;set;}

        /// <summary>
        /// 所属栏目ID
        /// </summary>
          public string BeLongToID{get;set;}

        /// <summary>
        ///模块标题
        /// </summary>
          public string Title{get;set;}


        /// <summary>
        /// 模块样式
        /// </summary>
          public string Styles{get;set;}


      


        /// <summary>
        /// 是否独占一行
        /// </summary>
          public int IsAlone{get;set;}


        /// <summary>
        /// 左边 还是右边
        /// </summary>
          public int LeftOrRight{get;set;}

        /// <summary>
        /// 留学还是移民
        /// </summary>
          public int AbroadOrLearn{get;set;}

        
        public string AboutMenuID { get; set; }


        public int Flag { get; set; }


        /// <summary>
        /// 序号
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
          public DateTime CreateTime{get;set;}

        /// <summary>
        /// 更新时间
        /// </summary>
          public DateTime UpdateTime { get;set ; }
    }
}
