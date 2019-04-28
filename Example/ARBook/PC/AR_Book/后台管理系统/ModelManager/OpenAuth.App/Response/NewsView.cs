using OpenAuth.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
namespace OpenAuth.App.Response
{
   public class NewsView
    {

        public  string Id { get; set; }



        public string Title { get; set; }

        /// <summary>
        /// /封面
        /// </summary>
        public string ImgUrl { get; set; }


        /// <summary>
        /// 所属栏目
        /// </summary>
        public string BelongToID { get; set; }
        /// <summary>
        /// 所属栏目
        /// </summary>
        public string BelongMenu
        {
            get;
            
            set;

        }


        /// <summary>
        ///关键字 
        /// </summary>
        public string KeyWords { get; set; }

        /// <summary>
        /// 简要概述
        /// </summary>
        public string Describe { get; set; }


        /// <summary>
        /// 详情
        /// </summary>
        public string Detail { get; set; }


        /// <summary>
        /// 是否在主页上显示
        /// </summary>
        public int IsOnPage { get; set; }
        public string OnPage
        {
            get
            {
                if (IsOnPage == 0)
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
        /// 浏览次数
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 资讯类型
        /// </summary>
        public int Cate { get; set; }


        public string CateDesc
        {
            get
            {

                if (Cate == 1)
                {
                    return "中学";
                }
                else if (Cate == 2)
                {
                    return "大学";
                }
                else if (Cate == 3)
                {
                    return "硕士";
                }


                return "";
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Flag { get; set; }


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
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }


        public static implicit operator NewsView(News module)
        {
            return module.MapTo<NewsView>();
        }
        public static implicit operator News(NewsView rolevm)
        {
            return rolevm.MapTo<News>();
        }

    }
}
