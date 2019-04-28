using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Repository.Domain
{


    /// <summary>
    /// 所应用书籍
    /// </summary>
   public class Book: Entity
    {

        /// <summary>
        /// 书籍ID
        /// </summary>
          public string Id { get;set;}
        /// <summary>
        /// 书籍名称
        /// </summary>
          public string Name{get;set;}
        /// <summary>
        /// 说明
        /// </summary>
          public string Remark{get;set;}

        /// <summary>
        /// ISBN书籍唯一编号
        /// </summary>
          public string ISBN{get;set;}

        /// <summary>
        /// 出版社
        /// </summary>
          public string PubHouse{get;set;}

        /// <summary>
        /// 图片
        /// </summary>
          public string Photo{get;set;}

        /// <summary>
        /// 标志
        /// </summary>
          public int Flag{get;set;}

        /// <summary>
        /// 创建时间
        /// </summary>
          public DateTime CreateTime{get;set;}

        /// <summary>
        ///更新时间
        /// </summary>
          public DateTime UpdateTime { get; set; }

    }
}
