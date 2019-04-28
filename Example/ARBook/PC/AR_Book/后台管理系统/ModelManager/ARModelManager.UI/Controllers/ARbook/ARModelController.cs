using ARModelManager.UI.Models.ARJsonModel;
using ARModelManager.UI.Models.SystemCode;
using Infrastructure;
using OpenAuth.App;
using QS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XCView.Core.Util;

namespace ARModelManager.UI.Controllers
{
    public class ARModelController : Controller
    {

        public ArModelManagerApp armApp { get; set; }

        // GET: ARModel
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Update()
        {
            return View();
        }


        public ActionResult Delete()
        {
            return View();
        }

        /// <summary>
        /// 提交答案接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitAnswer()
        {

            var deviceID = Request["deviceID"];

            var userID = Request["UserID"];

            var userName = Request["userName"];

            var modelKey = Request["modelKey"];

            var answer = Request["answer"];

            XCView.Core.Util.LogHelper.WriteInfo(string.Format("答案：{0};UserName:{1};ModelKey:{2}", answer,userName,modelKey));
            return Json(new {
                code = 0,
                msg ="答案上传成功"
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取指定模型信息
        /// </summary>
        /// <returns></returns>
        public ActionResult TestGetModelInfo()
        {


            var key = Request["keywords"];
         XCView.Core.Util.LogHelper.WriteInfo(string.Format("查找关键字：{0}",key));
            if (string.IsNullOrEmpty(key))
            {
                return Json(new {
                    code = -1,
                    msg="请求关键字不能为空"
                }, JsonRequestBehavior.AllowGet);
            }
            // LogHelper.WriteInfo("请求到达时间：" + DateTime.Now);

            


            #region  物件类型 数据格式
            RootModel modelData = new RootModel
            {
                Type = "WuJian",
                Name = "CL7型自行式铲运机",
                CNID = "图1-11",              
                SerId = "p1-11",      
                aburl  = "http://arbookresouce.73data.cn/p1-11?t="+ DateTime.Now.Ticks,
                AbObjectName = "TestGo",           
                TypeItem = new TypeItem {

                    Introduce = new Introduce {
                                Title = "铲运机介绍",
                                Info = "铲运机是一种能独立完成铲土、运土、卸土、填筑、场地平整的土方施工机械。其对道路要求较低，操纵灵活，具有生产效率较高的特点。"
                    },
                 
                    Structs = new Structs {
                            Title = "铲运机结构",
                            Struct = new List<StructItem> {
                                      new StructItem{
                                          Index= 1,
                                          Info= "驾驶室",
                                          GameObjectName= "jiashishi",
                                          LeftOrRight= false
                                   },
                                   new StructItem{
                                        Index = 2,
                                        Info = "前轮",
                                        GameObjectName = "qianlun",
                                        LeftOrRight = false
                                   },                            
                             
                                   new StructItem{
                                         Index= 3,
                                         Info="中央框架",
                                         GameObjectName= "zhongyangkuangjia",
                                         LeftOrRight= false
                                   },
                                   new StructItem{
                                         Index= 4,
                                         Info="铲斗",
                                         GameObjectName= "chandou",
                                         LeftOrRight= true
                                   },
                                  new StructItem{
                                         Index= 5,
                                         Info="尾架",
                                         GameObjectName= "weijia",
                                         LeftOrRight= false
                                   },

                            }


                },

                Sizes = new Sizes {
                     Title = "尺寸",
                      Size = new List<SizeItem> {
                          new SizeItem{
                              Index = 1,
                              Length = 9800,
                               ObjcetName = "总长",
                              FromGoName = "zongchang1",
                              ToGaName = "zongchang2"
                          },
                          new SizeItem{
                              Index = 2,
                              Length = 5900,
                               ObjcetName = "轮距",
                              FromGoName = "lunju1",
                              ToGaName = "lunju2"
                          },
                         new SizeItem{
                              Index = 3,
                              Length = 2980,
                              ObjcetName = "头高",
                              FromGoName = "tougao1",
                              ToGaName = "tougao2"
                          },
                                  new SizeItem{
                              Index = 4,
                              Length = 550,
                               ObjcetName = "尾高",
                              FromGoName = "weigao1",
                              ToGaName = "weigao2"
                          },
                      }
                },
                Video = new Video {
                               Title = "铲运机测试",
                               Info = "描述测试",
                               Url ="http://www.html5videoplayer.net/videos/toystory.mp4",
                              },
                 Practices = new TypeItemPractice {
                      AnswerImageUrl= "http://arbookresouce.73data.cn/wallhaven-637809.jpg",
                     PracticeTitle = "请正确匹配五个铲运机构建：",
                     IsShowAnswer = true,
                      PracObjects  = new List<TypePracticeItem> {

                           new TypePracticeItem{
                               Index =1,
                                Info ="驾驶室",
                            GameObjectName ="jiashishi",
                            LeftOrRight = true
                           },
                            new TypePracticeItem{
                                  Index =2,
                                Info ="前轮",
                            GameObjectName ="qianlun",
                            LeftOrRight = true
                           },



                      }



                 }
                 
                       },
                 TypeMethod = new TypeMethod {
                 },
                 TypeProcess = new TypeProcess {
                 },
                 TypeTable = new TypeTable {
                 },                                                    
            };
            #endregion
            #region 计算类型数据
            RootARCaculate calModelData = new RootARCaculate
            {
                   
              Type = "JiSuan",               
              Name ="基坑土方量计算",
              CNID="图1-12",
              aburl= "http://arbookresouce.73data.cn/p1-12",
              AbObjectName= "JiSuanTestGo",
              TypeCalculation = new CaculTypeCalculation {

                   OverallInfo = new OverallInfo {
                       Title = "基坑概念",
                       Info = "基坑是指长宽比小于或等于3的矩形土体",
                   },
                   Calculations = new Calculations {
                        Title = "土方量计算",
                         Info = "基坑土方量计算公式：V=H/6(A1+4A0+A2)",
                         Calcul = new List<Calcul> {
                              new Calcul{
                                      Index= 1,
                                      Info= "基坑下底的面积,m²",
                                      LeftOrRight= false,
                                      GameObjectName= "d001",
                                      GameObjectIndex= "A2"
                              },
                             new Calcul{
                                 Index= 2,
                                 Info= "基坑中截面的面积,m²",
                                 LeftOrRight= false,
                                 GameObjectName= "d002",
                                 GameObjectIndex= "A0"
                              },
                             new Calcul{
                                  Index= 3,
                                  Info= "基坑上底的面积,m²",
                                  LeftOrRight= true,
                                  GameObjectName= "d003",
                                  GameObjectIndex= "A1"
                              }

                         },
                         Sizes = new List<TypeSizes> {

                              new TypeSizes{
                                  Index= 4,
                                  Length= "H",
                                  FromGoName= "Dummy002",
                                  ToGaName= "Dummy001"
                              }
                         },                    
                   },

                    Practices = new Practice {
                         TextInfo = "某基坑平面尺寸如上图所示，坑深5.5m，四边均按1:0.4的坡度放坡，抗深范围内箱形基础的体积为2000m3。基坑开挖的土方量面积为#m3。",
                          TaskPicName = "wallhaven-637809.jpg",
                           TaskPicUrl = "http://arbookresouce.73data.cn/wallhaven-637809.jpg",
                            AnswerPicName = "1524478227471.jpg",
                             AnswerPicUrl = "http://arbookresouce.73data.cn/1524478227471.jpg",
                              IsShowAnswer = true,
                               Answers = new List<AnswerItem> {
                                   new AnswerItem{
                                        Index = 1,
                                         Answer = "10.52"
                                   }
                               }
                               

                    }


                },

                TypeItem = new CaculTypeItem(),
                TypeMethod = new CaculTypeMethod(),
                TypeProcess = new CaculTypeProcess(),
                TypeTable = new CaculTypeTable()
                 

            };

            #endregion
            #region 方式类型
            var methodModelData = new ArModelMethod()
            {
                      Type= "FangShi",
                      Name= "正铲挖土机作业方式",
                      CNID= "图1-16",
                      aburl= "http://arbookresouce.73data.cn/p1-16",
                      AbObjectName = "",
                      TypeCalculation = new MethodTypeCalculation(),
                      TypeItem = new MethodTypeItem(),
                      TypeMethod = new MethodTypeMethod {
                          Methods = new List<MethodsItem> {
                              new MethodsItem{

                                    Index= 1,
                                    Title= "正向挖土 反向卸土",
                                    Info= "正向开挖侧向卸土即挖土机向前进方向挖土，汽车停在侧面装土，此开挖方式因挖掘机卸土时回转角小、运输方便、生产效率高而应用较广。",
                                    GameObjectName= "zhengchanwajuejidonghua"
                              },
                              //new MethodsItem{

                              //    Index= 2,
                              //    Title= "正向挖土 侧向卸土",
                              //    Info= "正向开挖侧向卸土即挖土机向前进方向挖土，汽车停在侧面装土，此开挖方式因挖掘机卸土时回转角小、运输方便、生产效率高而应用较广。",
                              //     GameObjectName= "FuncTestGo"

                              //}
                          },
                           Practices = new MethodPractice {

                               TextInfo= "某单位在进行深基坑场地平整，请根据当前地形，将正确的作业工具拖动到场地中：",
                               IsShowAnswer= false,
                               TaskPicName= "wallhaven-637809.jpg",
                               TaskPicUrl= "http://arbookresouce.73data.cn/wallhaven-637809.jpg",
                               AnswerPicName= "1524478227471.jpg",
                               AnswerPicUrl= "http://arbookresouce.73data.cn/1524478227471.jpg",
                               PracObjects = new List<MethodPracticeItem> {

                                    new MethodPracticeItem   { Index= 1, Url= "http://arbookresouce.73data.cn/1524478227471.jpg" },
                                    new MethodPracticeItem { Index= 2, Url= "http://arbookresouce.73data.cn/1524478227471.jpg" },
                                    new MethodPracticeItem { Index= 3, Url= "http://arbookresouce.73data.cn/1524478227471.jpg" },
                                    new MethodPracticeItem { Index= 4, Url= "http://arbookresouce.73data.cn/1524478227471.jpg" }

                                },
                                Answers = new List<string> {
                                    "1",
                                    "4"
                                }

                           },
                            


                       },
                      TypeTable =new MethodTypeTable(),
                      TypeProcess = new MethodTypeProcess()


            };

            #endregion
            #region 流程类
            var processModelData = new ARModelProcess()
            {
                Type= "LiuCheng",
                Name= "水泥土桩墙工程施工工艺",
                CNID= "图1-18",
                aburl= "http://arbookresouce.73data.cn/p1-18",
                AbObjectName= "ProcessTestGo",

                TypeCalculation=new ProcessTypeCalculation {
                       },
                TypeItem= new ProcessTypeItem {
                       },
                TypeMethod=new ProcessTypeMethod {
                       },
                TypeTable=new ProcessTypeTable {
                        },

                 TypeProcess = new ProcessTypeProcess {
                      AbObjectName = "shencengdanzhoujiaobanjidonghua",
                      Frames = "20-38,58-102,118-130,151-179,191-220,244-320",
                     AnimStates = new List<AnimStatesItem> {

                          new AnimStatesItem{
                              Index= 1,
                              StateInfo= "（1）定位。深层搅拌机桩架到达指定桩位、对中"
                          },
                          new AnimStatesItem{
                              Index= 2,
                              StateInfo= "（2）预搅下沉。 放松起重机的钢线绳，使搅拌机沿导向架搅拌切土下沉"
                          },
                          new AnimStatesItem{
                               Index= 3,
                               StateInfo= "（3）提升喷浆搅拌。开启灰浆泵将水泥浆压入地基土中，并且边喷浆、边旋转"
                          },
                          new AnimStatesItem{
                              Index= 4,
                              StateInfo= "（4）重复下沉搅拌。为使软土和水泥浆搅拌均匀，可再次将搅拌头边旋转边沉入土中"
                          },
                          new AnimStatesItem{
                                Index= 5,
                                StateInfo= "（5）重复提升搅拌。至设计加固深度后再将搅拌头提升出地面"
                          },
                          new AnimStatesItem{
                              Index= 6,
                              StateInfo= "（6）成桩结束。成桩结束后，桩架移至下一桩位施工"
                          },
                     },
                      Practices = new ProcessPractices {
                                  TextInfo= "水泥土桩墙工程的施工工艺流程为",
                                  TaskPicName= "wallhaven-637809.jpg",
                                  TaskPicUrl= "http://arbookresouce.73data.cn/wallhaven-637809.jpg",
                                  AnswerPicName= "1524478227471.jpg",
                                  AnswerPicUrl= "http://arbookresouce.73data.cn/1524478227471.jpg",
                                  IsShowAnswer= true,
                                  PracObjects = new List<string> {
                                       "重复下沉搅拌",
                                       "成桩结束",
                                       "重复提升搅拌"

                                   },
                          Answers = new List<AnswerItemProcess> {
                              new AnswerItemProcess{

                                  Index= 1, Text= "重复下沉搅拌"
                              },
                              new AnswerItemProcess{
                                  Index= 2, Text= "成桩结束"
                              },
                              new AnswerItemProcess{
                                  Index= 3, Text= "重复提升搅拌"
                              }

                          }



                      }


                 }
                 
            };
            #endregion
            #region  图表类
            var tableModelData = new ARModelTable()
            {

                Type = "TuBiao",
                Name = "填土压实的影响因素",
                CNID = "图1-13",
                aburl = "http://arbookresouce.73data.cn/p1-13",

                TypeCalculation = new TableTypeCalculation(),
                TypeItem = new TableTypeItem(),
                TypeMethod = new TableTypeMethod(),
                TypeTable = new TableTypeTable
                {
                    Effect = new List<EffectItem> {
                 new  EffectItem{
                    Title= "压实功作用",
                    BackgroundUrl= "http://arbookresouce.73data.cn/BackTemplate.png",
                    DynamicPicUrl= "http://arbookresouce.73data.cn/DynamicTemplate.png",
                    GameObjectName= "yalujidonghua"
                  },
                  new  EffectItem{
                    Title= "土的水含量",
                    BackgroundUrl= "",
                    DynamicPicUrl= "",
                    GameObjectName= "GameObjectName"

                  },
                 new  EffectItem {
                    Title= "每层铺土厚度",
                    BackgroundUrl= "",
                    DynamicPicUrl= "",
                    GameObjectName= "GameObjectName"
                  }
                },
                    Practices = new Practices
                    {
                        TextInfo = "影响土压实的因素有# # #",
                        TaskPicName = "wallhaven-637809.jpg",
                        TaskPicUrl = "http://arbookresouce.73data.cn/wallhaven-637809.jpg",
                        AnswerPicName = "1524478227471.jpg",
                        AnswerPicUrl = "http://arbookresouce.73data.cn/1524478227471.jpg",
                        IsShowAnswer = true,
                        Answers = new List<AnswerItem> {
                    new AnswerItem {
                      Index= 1,
                      Answer= "因素1"
                    },
                    new AnswerItem {
                      Index= 2,
                      Answer= "因素2"
                    },
                    new AnswerItem {
                      Index= 3,
                      Answer= "因素3"
                    }
                  }
                    }



                },
                TypeProcess = new TableTypeProcess(),
            };
            #endregion      
           

            switch (key)
            {
                case "图1-11":
                    return Json(new {
                      code = 0,
                      msg="获取成功",
                      data =  modelData
                    } , JsonRequestBehavior.AllowGet);
                case "图1-12":
                    return Json(new
                    {
                        code = 0,
                        msg = "获取成功",
                        data = calModelData
                    }, JsonRequestBehavior.AllowGet);
                case "图1-16":
                    return   Json(new
                    {
                        code = 0,
                        msg = "获取成功",
                        data = methodModelData
                    }, JsonRequestBehavior.AllowGet);
                case "图1-18":
                    return Json(new
                    {
                        code = 0,
                        msg = "获取成功",
                        data = processModelData
                    }, JsonRequestBehavior.AllowGet);
                case "图1-13":
                    return Json(new
                    {
                        code = 0,
                        msg = "获取成功",
                        data = tableModelData
                    }, JsonRequestBehavior.AllowGet);
                default:
                    return Json(new {
                        code = -1,
                        msg ="找不到数据"

                    }, JsonRequestBehavior.AllowGet);
            }

            //return Json(modelData, JsonRequestBehavior.AllowGet);
            //return View();
        }



        public ActionResult GetModelInfo()
        {
            var key = Request["keywords"];
           XCView.Core.Util.LogHelper.WriteInfo(string.Format("查找关键字：{0}", key));
            if (string.IsNullOrEmpty(key))
            {
                return Json(new
                {
                    code = -1,
                    msg = "请求关键字不能为空"
                }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var model = armApp.GetARModel(key);



                if (model != null && !string.IsNullOrEmpty(model.Resource))
                {

                    switch (model.Type)
                    {

                        case (int)ModelTypeCode.WuJian:

                            RootModel rootmodel = PrepareWuJianModel(model);

                            return Json(new {
                                code = 0,
                                msg = "获取成功",
                                data = rootmodel
                            } , JsonRequestBehavior.AllowGet);


                        case (int)ModelTypeCode.Cacul:
                            RootARCaculate rootcalmodel = PrepareJiSuanModel(model);

                            return Json(new
                            {
                                code = 0,
                                msg = "获取成功",
                                data = rootcalmodel
                            }
                                , JsonRequestBehavior.AllowGet);

                        case (int)ModelTypeCode.Method:

                            ArModelMethod rootMethodModel = PrepareMethodModel(model);
                            return Json(new
                            {
                                code = 0,
                                msg = "获取成功",
                                data = rootMethodModel
                            }, JsonRequestBehavior.AllowGet);

                        case (int)ModelTypeCode.Process:

                            ARModelProcess rootpromodel = PrepareLiuChengModel(model);
                            return Json(new
                            {
                                code = 0,
                                msg = "获取成功",
                                data = rootpromodel
                            }, JsonRequestBehavior.AllowGet);

                        case (int)ModelTypeCode.Table:

                            ARModelTable roottablemodel = PrepareTuBiaoModel(model);

                            if (roottablemodel != null)
                            {
                                roottablemodel.TypeTable.Effect.ForEach(x =>
                                {
                                    if (key.Equals(x.Key))
                                    {
                                        x.IsOn = true;
                                    }
                                    else
                                    {
                                        x.IsOn = false;
                                    }


                                });
                            }

                            return Json(new
                            {
                                code = 0,
                                msg = "获取成功",
                                data = roottablemodel
                            }, JsonRequestBehavior.AllowGet);
                    }
                }



            }
            catch (Exception ex)
            {
                XCView.Core.Util.LogHelper.WriteException("TestGetModelInfo()", ex);
            }

          


            return Json(new
            {
                code = 0,
                msg = "暂无数据"
            }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 完善图表模型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static ARModelTable PrepareTuBiaoModel(OpenAuth.Repository.Domain.ARModel model)
        {
            var roottablemodel = JsonHelper.Instance.Deserialize<ARModelTable>(model.Resource);


            if (roottablemodel != null)
            {
                roottablemodel.aburl = string.Format("{0}/{1}", ConfigUtils.qiniurl, roottablemodel.aburl);// (ConfigUtils.qiniurl + "/" + rootmodel);

                ///动态装配 视频URL Practices
                if (roottablemodel.TypeTable != null && roottablemodel.TypeTable.Practices != null)
                {
                    if (!string.IsNullOrEmpty(roottablemodel.TypeTable.Practices.TaskPicName))
                    {
                        roottablemodel.TypeTable.Practices.TaskPicUrl = string.Format("{0}/{1}", ConfigUtils.qiniurl, roottablemodel.TypeTable.Practices.TaskPicName);
                    }
                }

                if (roottablemodel.TypeTable != null && roottablemodel.TypeTable.Practices != null && string.IsNullOrEmpty(roottablemodel.TypeTable.Practices.AnswerPicName))
                {

                    roottablemodel.TypeTable.Practices.AnswerPicUrl = string.Format("{0}/{1}", ConfigUtils.qiniurl, roottablemodel.TypeTable.Practices.AnswerPicName);
                }


                if (roottablemodel.TypeTable != null && roottablemodel.TypeTable.Practices != null && roottablemodel.TypeTable.Effect != null)
                {
                    roottablemodel.TypeTable.Effect.ForEach(x =>
                    {
                        x.BackgroundUrl = string.Format("{0}/{1}", ConfigUtils.qiniurl, x.BackgroundUrl);

                        x.DynamicPicUrl = string.Format("{0}/{1}", ConfigUtils.qiniurl, x.DynamicPicUrl);
                    });
                }





            }

            return roottablemodel;
        }


        /// <summary>
        /// 流程模型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static ARModelProcess PrepareLiuChengModel(OpenAuth.Repository.Domain.ARModel model)
        {
            var rootpromodel = JsonHelper.Instance.Deserialize<ARModelProcess>(model.Resource);


            if (rootpromodel != null)
            {
                rootpromodel.aburl = string.Format("{0}/{1}", ConfigUtils.qiniurl, rootpromodel.aburl);// (ConfigUtils.qiniurl + "/" + rootmodel);

                ///动态装配 视频URL Practices
                if (rootpromodel.TypeProcess != null && rootpromodel.TypeProcess.Practices != null)
                {
                    if (!string.IsNullOrEmpty(rootpromodel.TypeProcess.Practices.TaskPicName))
                    {
                        rootpromodel.TypeProcess.Practices.TaskPicUrl = string.Format("{0}/{1}", ConfigUtils.qiniurl, rootpromodel.TypeProcess.Practices.TaskPicName);
                    }
                }

                if (rootpromodel.TypeProcess != null && rootpromodel.TypeProcess.Practices != null && string.IsNullOrEmpty(rootpromodel.TypeProcess.Practices.AnswerPicName))
                {

                    rootpromodel.TypeProcess.Practices.AnswerPicUrl = string.Format("{0}/{1}", ConfigUtils.qiniurl, rootpromodel.TypeProcess.Practices.AnswerPicName);
                }





            }

            return rootpromodel;
        }
        /// <summary>
        /// 方式模型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static ArModelMethod PrepareMethodModel(OpenAuth.Repository.Domain.ARModel model)
        {
            var rootMethodModel = JsonHelper.Instance.Deserialize<ArModelMethod>(model.Resource);


            if (rootMethodModel != null)
            {
                rootMethodModel.aburl = string.Format("{0}/{1}", ConfigUtils.qiniurl, rootMethodModel.aburl);

                if (rootMethodModel.TypeMethod != null && rootMethodModel.TypeMethod.Practices != null && string.IsNullOrEmpty(rootMethodModel.TypeMethod.Practices.TaskPicName))
                {
                    rootMethodModel.TypeMethod.Practices.TaskPicUrl = string.Format("{0}/{1}", ConfigUtils.qiniurl, rootMethodModel.TypeMethod.Practices.TaskPicName);

                }
                if (rootMethodModel.TypeMethod != null && rootMethodModel.TypeMethod.Practices != null && string.IsNullOrEmpty(rootMethodModel.TypeMethod.Practices.AnswerPicName))
                {
                    rootMethodModel.TypeMethod.Practices.AnswerPicUrl = string.Format("{0}/{1}", ConfigUtils.qiniurl, rootMethodModel.TypeMethod.Practices.AnswerPicName);

                }
                if (rootMethodModel.TypeMethod != null && rootMethodModel.TypeMethod.Practices != null && rootMethodModel.TypeMethod.Practices.PracObjects != null)
                {

                    rootMethodModel.TypeMethod.Practices.PracObjects.ForEach(x =>
                    {
                        if (string.IsNullOrEmpty(x.Url))
                        {
                            x.Url = string.Format("{0}/{1}", ConfigUtils.qiniurl, x.Url);
                        }
                    });

                }



            }

            return rootMethodModel;
        }

        /// <summary>
        ///  完善物件类型给前端
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static RootARCaculate PrepareJiSuanModel(OpenAuth.Repository.Domain.ARModel model)
        {
            var rootcalmodel = JsonHelper.Instance.Deserialize<RootARCaculate>(model.Resource);


            if (rootcalmodel != null)
            {
                rootcalmodel.aburl = string.Format("{0}/{1}", ConfigUtils.qiniurl, rootcalmodel.aburl);// (ConfigUtils.qiniurl + "/" + rootmodel);





                ///动态装配 视频URL Practices
                if (rootcalmodel.TypeCalculation != null && rootcalmodel.TypeCalculation.Practices != null)
                {
                    if (!string.IsNullOrEmpty(rootcalmodel.TypeCalculation.Practices.TaskPicName))
                    {
                        rootcalmodel.TypeCalculation.Practices.TaskPicUrl = string.Format("{0}/{1}", ConfigUtils.qiniurl, rootcalmodel.TypeCalculation.Practices.TaskPicName);
                    }
                }
                else
                {
                    rootcalmodel.TypeCalculation.Practices.TaskPicUrl = string.Empty;
                }

                if (rootcalmodel.TypeCalculation != null && rootcalmodel.TypeCalculation.Practices != null && !string.IsNullOrEmpty(rootcalmodel.TypeCalculation.Practices.AnswerPicName))
                {

                    rootcalmodel.TypeCalculation.Practices.AnswerPicUrl = string.Format("{0}/{1}", ConfigUtils.qiniurl, rootcalmodel.TypeCalculation.Practices.AnswerPicName);
                }
                else
                {
                    rootcalmodel.TypeCalculation.Practices.AnswerPicUrl = string.Empty;
                }
                if (rootcalmodel.TypeCalculation != null && rootcalmodel.TypeCalculation.Calculations != null&& rootcalmodel.TypeCalculation.Calculations.Calcul != null)
                {
                    rootcalmodel.TypeCalculation.Calculations.Calcul.ForEach(x =>
                    {

                        if (!string.IsNullOrEmpty(x.Audio))
                        {
                            x.Audio = string.Format("{0}/{1}", ConfigUtils.qiniurl, x.Audio);
                        }
                        else
                        {
                            x.Audio = string.Empty;
                        }
                        

                    });
                }
                if (rootcalmodel.TypeCalculation != null && rootcalmodel.TypeCalculation.Calculations != null && rootcalmodel.TypeCalculation.Calculations.Sizes != null)
                {
                    rootcalmodel.TypeCalculation.Calculations.Sizes.ForEach(x =>
                    {
                        if (!string.IsNullOrEmpty(x.Audio))
                        {
                            x.Audio = string.Format("{0}/{1}", ConfigUtils.qiniurl, x.Audio);
                        }
                        else
                        {
                            x.Audio = string.Empty;
                        }

                    });
                }



            }

            return rootcalmodel;
        }
        /// <summary>
        /// 完善计算类型给
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static RootModel PrepareWuJianModel(OpenAuth.Repository.Domain.ARModel model)
        {
            var rootmodel = JsonHelper.Instance.Deserialize<RootModel>(model.Resource);


            if (rootmodel != null)
            {
                rootmodel.aburl = string.Format("{0}/{1}?T={2}", ConfigUtils.qiniurl, rootmodel.aburl,DateTime.Now.Ticks);// (ConfigUtils.qiniurl + "/" + rootmodel);

                ///动态装配 视频URL
                if (rootmodel.TypeItem != null && rootmodel.TypeItem.Video != null)
                {
                    if (!string.IsNullOrEmpty(rootmodel.TypeItem.Video.Url))
                    {
                        rootmodel.TypeItem.Video.Url = string.Format("{0}/{1}", ConfigUtils.qiniurl, rootmodel.TypeItem.Video.Url);
                    }
                }

                if (rootmodel.TypeItem != null && rootmodel.TypeItem.Practices != null && string.IsNullOrEmpty(rootmodel.TypeItem.Practices.TaskPicName))
                {
                    rootmodel.TypeItem.Practices.AnswerImageUrl = string.Format("{0}/{1}", ConfigUtils.qiniurl, rootmodel.TypeItem.Practices.AnswerPicName);
                    rootmodel.TypeItem.Practices.AnswerPicUrl = string.Format("{0}/{1}", ConfigUtils.qiniurl, rootmodel.TypeItem.Practices.AnswerPicName);
                }
                if (rootmodel.TypeItem != null && rootmodel.TypeItem.Introduce != null)
                {
                    if (!string.IsNullOrEmpty(rootmodel.TypeItem.Introduce.Audio))
                    {
                        rootmodel.TypeItem.Introduce.Audio = string.Format("{0}/{1}", ConfigUtils.qiniurl, rootmodel.TypeItem.Introduce.Audio);
                    }
                    else
                    {
                        rootmodel.TypeItem.Introduce.Audio = string.Empty;
                    }
                }
                    if (rootmodel.TypeItem != null && rootmodel.TypeItem.Structs != null && rootmodel.TypeItem.Structs.Struct != null)
                {
                    rootmodel.TypeItem.Structs.Struct.ForEach(x =>
                    {

                        if (!string.IsNullOrEmpty(x.Audio))
                        {
                            x.Audio = string.Format("{0}/{1}", ConfigUtils.qiniurl, x.Audio);
                        }
                        else
                        {
                            x.Audio = string.Empty;
                        }


                    });
                }
                if (rootmodel.TypeItem != null && rootmodel.TypeItem.Sizes != null  && rootmodel.TypeItem.Sizes.Size != null)
                {
                    rootmodel.TypeItem.Sizes.Size.ForEach(x =>
                    {
                        if (!string.IsNullOrEmpty(x.Audio))
                        {
                            x.Audio = string.Format("{0}/{1}", ConfigUtils.qiniurl, x.Audio);
                        }
                        else
                        {
                            x.Audio = string.Empty;
                        }

                    });
                }



            }

            return rootmodel;
        }
    }
}