using OpenAuth.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
namespace OpenAuth.App.Response
{
  public  class NavMenuView
    {
        /// <summary>
        /// 唯一标志
        /// </summary>
        public string Id { get; set; }



        public string ParentId { get; set; }
        public string Name { get
            {
                return Title;
            } }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// 是否在导航栏上显示
        /// </summary>
        public string IsOnNav { get; set; }

        /// <summary>
        /// 是否显示在主页
        /// </summary>
        public string isOnPage { get; set; }


        /// <summary>
        /// Href
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// 样式
        /// </summary>
        public string Styles { get; set; }

        /// <summary>
        /// 标志信息
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// 出国或留学
        /// </summary>
        public string AbroadOrLearn { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public string Sort { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string UpdateTime { get; set; }



        public static implicit operator NavMenuView(Menu module)
        {
            return module.MapTo<NavMenuView>();
        }
        public static implicit operator Menu(NavMenuView rolevm)
        {
            return rolevm.MapTo<Menu>();
        }

    }
}
