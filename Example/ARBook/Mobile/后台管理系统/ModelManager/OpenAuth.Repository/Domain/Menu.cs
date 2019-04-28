using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Repository.Domain
{
  public  class Menu:Entity
    {


        /// <summary>
        /// 唯一标志
        /// </summary>
          public new string Id{get;set;}

        /// <summary>
        /// 上级菜单
        /// </summary>
         public string ParentId { get; set; }


        /// <summary>
        /// 标题
        /// </summary>
          public string Title{get;set;}


        /// <summary>
        /// 是否在导航栏上显示
        /// </summary>
          public int IsOnNav{get;set;}


        public string OnNav
        {
            get
            {
                if (IsOnNav == 0)
                {
                    return "否";
                }
                else
                {
                    return "是";
                }
            }
        }
        /// <summary>
        /// 是否显示在主页
        /// </summary>
        public int isOnPage{get;set;}


          public string OnPage
            {
                get
                {
                    if (isOnPage == 0)
                    {
                        return "否";
                    }
                    else
                    {
                        return "是";
                    }
                }
            }

        /// <summary>
        /// Href
        /// </summary>
          public string Href{get;set;}

        /// <summary>
        /// 样式
        /// </summary>
          public string Styles{get;set;}

        /// <summary>
        /// 标志信息
        /// </summary>
          public int Flag{get;set;}


        public string FlagDesc
        {
            get
            {
                if (Flag == 1)
                {
                    return "正常";
                }
                else
                {
                    return "禁用";
                }
            }
        }

        /// <summary>
        /// 出国或留学
        /// </summary>
          public int AbroadOrLearn{get;set;}


        public string AbroadOrLearnDesc
        {
            get
            {
                if (AbroadOrLearn == 1)
                {
                    return "移民";
                }
                else if (AbroadOrLearn ==2)
                {
                    return "留学";
                }
                else
                {
                    return "未指明";
                }
            }
        }


        /// <summary>
        /// 排序
        /// </summary>
          public int Sort{get;set;}


        /// <summary>
        /// 创建时间
        /// </summary>
          public DateTime CreateTime{get;set;}


        public string CreateTimeDesc
        {
            get
            {
                return CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
                
        }

        /// <summary>
        /// 修改时间
        /// </summary>
          public DateTime UpdateTime { get; set; }


        public string UpdateTimeDesc
        {
            get
            {
                return UpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }

        }
    }
}
