using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ARModelManager.UI.Models.SystemCode
{
    public enum ModelTypeCode
    {

        /// <summary>
        /// 1 ： 物件类型
        /// </summary>
        [Description("物件类型")]
        WuJian = 1,
        /// <summary>
        /// 2 ：计算类型
        /// </summary>
        [Description("计算类型")]
        Cacul = 2,
        /// <summary>
        /// 3:方式类型
        /// </summary>
        [Description("方式类型")]
        Method = 3,
        /// <summary>
        /// 4:方式类型
        /// </summary>
        [Description("图表类型")]
        Table = 4,

        /// <summary>
        /// 5:方式类型
        /// </summary>
        [Description("流程类型")]
        Process = 5,

    }
}