using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARModelManager.UI.Models.ARJsonModel
{
 
    public class TableTypeCalculation
    {
    }

    public class TableTypeItem
    {
    }

    public class TableTypeMethod
    {
    }

    public class EffectItem
    {
        /// <summary>
        /// 压实功作用
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 锚点
        /// </summary>
        public string Key { get; set; }



        public bool IsOn
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string BackgroundUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DynamicPicUrl { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public string GameObjectName { get; set; }
    }

    public class Practices
    {
        /// <summary>
        /// 影响土压实的因素有# # #
        /// </summary>
        public string TextInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TaskPicName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TaskPicUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AnswerPicName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AnswerPicUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsShowAnswer { get; set; }


        public List<AnswerItem> Answers
        {
            get;set;
        }
    }

    public class TableTypeTable
    {
        /// <summary>
        /// 
        /// </summary>
        public List<EffectItem> Effect { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Practices Practices { get; set; }
    }

    public class TableTypeProcess
    {
    }

    public class ARModelTable
    {
        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 填土压实的影响因素
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图1-13
        /// </summary>
        public string CNID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string aburl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TableTypeCalculation TypeCalculation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TableTypeItem TypeItem { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TableTypeMethod TypeMethod { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TableTypeTable TypeTable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TableTypeProcess TypeProcess { get; set; }
    }
}