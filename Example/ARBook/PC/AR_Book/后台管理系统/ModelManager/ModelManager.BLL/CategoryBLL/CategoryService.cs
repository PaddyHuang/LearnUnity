using ModelManager.Domain;
using ModelManager.Domain.Entitys;
using ModelManager.Domain.SystemCode;
using ModelManager.Domain.ViewModel.ServerResponse.ZTree;
using ModelManager.Respository.AllRespository.CategoryDAL;
using QS.Common;
using QS.Common.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCView.Core.Extensions;
using XCView.Core.Util;

namespace ModelManager.BLL.CategoryBLL
{
   public class CategoryService:BaseBLL,ICategoryService
    {

       private readonly ICategoryRespository _categoryRespository;


       public CategoryService(ICategoryRespository categoryRespository)
       {
           _categoryRespository = categoryRespository;
       }


        public Domain.XCResult<bool> AddCategory(Domain.ViewModel.ClientRequest.CategoryRequest request)
        {

            if (request.ID.IsNullOrEmpty())
            {
                request.ID = Guid.NewGuid().ToString("N");
            }
            try
            {
                   var allcates = _categoryRespository.GetAllCategory();

                ///目录名称不能为空
                if (request.Name.IsNullOrEmpty())
                {
                    return Result(false, Domain.ErrorCode.cate_name_isnull);
                }

                ///目录序列不能为空
                if (request.CateNum.IsNullOrEmpty())
                {
                    return Result(false, Domain.ErrorCode.cate_num_isnull);
                }

                //上级目录ID不为空,目录名称不为空查找上级目录
                if (request.ParentID.IsNullOrEmpty() && !request.ParentName.IsNullOrEmpty())
                {
                 

                    var existCate = allcates.FirstOrDefault(x =>!string.IsNullOrEmpty(x.Name) && x.Name.Equals(request.ParentName));
                    if (existCate != null)
                    {
                        request.ParentID = existCate.ID;
                    }
                    else
                    {
                        return Result(false);
                    }
                    
                }

                ///检验名称是否重复
                var specialNameCate = allcates.FirstOrDefault(x => !string.IsNullOrEmpty(x.Name) && x.Name.Equals(request.Name));
                if (specialNameCate != null)
                {
                    return Result(false, Domain.ErrorCode.cate_name_repeat_fail);
                }


                if(!request.LevelName.IsNullOrEmpty())
                {
                    switch (request.LevelName)
                    {
                        case "书":
                            request.Level = (int)LevelCode.Book;
                            break;
                        case "章":
                            request.Level = (int)LevelCode.Zhang;
                            break;
                        case "节":
                            request.Level = (int)LevelCode.Jie;
                            break;
                        case "条":
                            request.Level = (int)LevelCode.Tiao;
                            break;
                        case "款":
                            request.Level = (int)LevelCode.Kuan;
                            break;
                        default:
                            request.Level = (int)LevelCode.Book;
                            break;
                    }
                }


  



                Category cate = new Category
                {
                     ID = request.ID,
                     Name = request.Name,
                     CateNum = request.CateNum,
                     Book = request.Book,
                     ParentID = request.ParentID??"0",
                     Keywords = request.Keywords,
                     CateType = request.CateType,
                     Flag = (int)FlagCode.Normal,
                      IsShowThumb = 0, 
                     Level =  request.Level,
                     CreateTime = DateTime.Now,
                     UpdateTime = DateTime.Now

                };



               var result = _categoryRespository.AddCategory(cate);


               if (result)
               {

                   FlushCachedCategoryList();
                   return Result(result);
               }
               else
               {
                   return Result(false, Domain.ErrorCode.cate_add_fail);
               }



               //return Result(true);

            }

            catch (Exception ex)
            {
                LogHelper.WriteException("AddCategory(Domain.ViewModel.ClientRequest.CategoryRequest request)", ex);

                return Result(false,Domain.ErrorCode.sys_fail);
            }

        }

        public Domain.XCResult<bool> UpdateCategory(Domain.ViewModel.ClientRequest.CategoryRequest request)
        {


            if (request.ID.IsNullOrEmpty())
            {
                return Result(false, Domain.ErrorCode.sys_param_format_error);
            }


            try
            {


                var cates = _categoryRespository.GetAllCategory();

                var oldCate = cates.FirstOrDefault(x => x.Name.Equals(request.Name));


                if (oldCate != null && !oldCate.ID.Equals(request.ID))                
                {
                    return Result(false, Domain.ErrorCode.cate_update_name_repeat_fail);
                }

                var existModel = cates.FirstOrDefault(x => x.ID.Equals(request.ID));


                if (existModel == null)
                {
                    return Result(false, Domain.ErrorCode.sys_param_format_error);
                }



                Category cate = new Category
                {
                     ID = request.ID,
                     Name = string.IsNullOrEmpty(request.Name)?existModel.Name:request.Name,
                     Book = request.Book,
                     CateNum = string.IsNullOrEmpty(request.CateNum) ? existModel.CateNum : request.CateNum,
                     Level =request.Level,
                     ParentID =string.IsNullOrEmpty( request.ParentID)?existModel.ParentID:request.ParentID,
                     CateType = request.CateType,
                     UpdateTime = DateTime.Now,
                     Flag = 0,
                     Keywords = request.Keywords                         
                };



              var result =   _categoryRespository.UpdateCategory(cate);


              if (result)
              {

                  FlushCachedCategoryList();
                  return Result(result);
              }

              else
              {
                  return Result(false, Domain.ErrorCode.cate_update_name_repeat_fail);
              }
            
            }
            catch (Exception ex)
            {
                LogHelper.WriteException("UpdateCategory(Domain.ViewModel.ClientRequest.CategoryRequest request)", ex);
                return Result(false, Domain.ErrorCode.sys_fail);

            }

            throw new NotImplementedException();
        }

        public Domain.XCResult<bool> DeleteCategory(string ids)
        {


            if (ids.IsNullOrEmpty())
            {
                return Result(false, Domain.ErrorCode.sys_param_format_error);
            }


            try
            {


                var result = _categoryRespository.DeleteCategory(ids);



                if (result)
                {

                    FlushCachedCategoryList();
                    return Result(true);
                }


            }
            catch (Exception ex)
            {

                LogHelper.WriteException(" DeleteCategory(string ids)", ex);

                return Result(false, Domain.ErrorCode.sys_fail);
 
            }



            return Result(false,Domain.ErrorCode.delete_cate_fail);


            //throw new NotImplementedException();
        }

        //public Domain.XCResult<List<Domain.Entitys.Category>> GetAllCategory()
        //{

        //    List<Category> cates = new List<Category>();


        //    try
        //    {
        //        cates = _categoryRespository.GetAllCategory();
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteException("GetAllCategory()", ex);

        //        return Result(cates, Domain.ErrorCode.sys_fail);
        //    }


        //    return Result(cates);

        //  //  throw new NotImplementedException();
        //}



       public Domain.XCResult<List<Category>> GetAllCategory()
        {

            List<Category> cates = new List<Category>();


            try
            {
                cates = _categoryRespository.GetAllCategory();


                cates = cates.Select(x =>{
                           return  new Category
                            {
                                 ID = x.ID,
                                 CateType = x.CateType,
                                 Book = x.Book,
                                 CateNum = x.CateNum,
                                 CreateTime = x.CreateTime,
                                 Flag = x.Flag,
                                 Keywords = x.Keywords,
                                 Level = x.Level,
                                 Name = x.Name,
                                 ParentID = x.ParentID,
                                 UpdateTime = x.UpdateTime,
                                 IsShowThumb = x.IsShowThumb,
                                 Url = string.Format(@"http://{0}/Model/GetModelList?keyword={1}&name={2}", GlobalData.Instance.Authority, x.Keywords ?? "", x.Name ?? "")


                            };
                
                }).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.WriteException("", ex);


                return Result(cates, Domain.ErrorCode.sys_fail);
            }

            return Result(cates);
           // throw new NotImplementedException();
        }

       public Domain.XCResult<List<Domain.ViewModel.ServerResponse.ZTree.QSTree>> GetTreeView()
        {


            List<QSTree> qsTrees = new List<QSTree>();


            try
            {
               var  cates = _categoryRespository.GetAllCategory();


               qsTrees = cates.Select(x =>
               {

                   var parent = cates.FirstOrDefault(p => p.ID.Equals(x.ParentID));

                   string parentName = string.Empty;


                   if (parent != null)
                   {
                       parentName = parent.Name;
                   }

                   return new QSTree
                   {
                        ID = x.ID,
                        ParentID = x.ParentID,
                        Name = x.Name,
                        ParentName = parentName,
                        Checked = false,
                        Open = true,   
                        Level = x.Level,
                        Keywords = x.Keywords,
                        Num = x.CateNum,
                        Icon = string.Empty, 
                        IsShowThumb = x.IsShowThumb
                                                    
                   };

               }).OrderBy(x=>x.Level).ThenBy(x=>x.Num).ToList();

            }
            catch (Exception ex)
            {
                LogHelper.WriteException("GetTreeView()", ex);

                return Result(qsTrees,Domain.ErrorCode.sys_fail);
            }
            return Result(qsTrees);


          //  throw new NotImplementedException();
        }

       /// <summary>
       /// 递归获取目录树
       /// </summary>
       /// <param name="source"></param>
       /// <param name="id"></param>
       /// <param name="trees"></param>
       /// <returns></returns>
      public  Domain.XCResult<List<Domain.ViewModel.ServerResponse.ZTree.QSTree>> GetTreeView(List<Category> source, string id,List<QSTree> trees)
        {
          ///  List<QSTree> trees = new List<QSTree>();


            

            try
            {

                if (trees == null)
                {
                    trees = new List<QSTree>();
                }

              //  var cates = _categoryRespository.GetAllCategory();


                var curcate = source.FirstOrDefault(x => x.ID.Equals(id));


                ///获取当前类目
                if (curcate != null)
                {
                    var parent = source.FirstOrDefault(p => p.ID.Equals(curcate.ParentID));

                   string parentName = string.Empty;


                  


                   if (parent != null)
                   {
                       parentName = parent.Name;
                   }

                   var childrenNodes = source.Where(x => x.ParentID.Equals(curcate.ID)).ToList();
                    trees.Add(   new QSTree
                       {
                           ID = curcate.ID,
                           ParentID = curcate.ParentID,
                           Name = curcate.Name,
                           ParentName = parentName,
                           Checked = false,
                           Open = true,
                           Keywords = curcate.Keywords,
                           Level = curcate.Level,
                           Num = curcate.CateNum,
                           Icon = string.Empty,
                           hasChild = (childrenNodes.Count>0),
                           IsShowThumb = curcate.IsShowThumb
                       });


          

                    if (childrenNodes.Count > 0)
                    {
                        foreach (var childNode in childrenNodes)
                        {
                           GetTreeView(source, childNode.ID, trees);
                        }
                    }



                }
            
            }
            catch (Exception ex)
            {
                LogHelper.WriteException("", ex);
            }




            return Result(trees);
        }



        public Domain.XCResult<Category> GetCategoryByID(string ID)
        {


            Category cate = null;


            if (ID.IsNullOrEmpty())
            {
                return Result(cate, Domain.ErrorCode.sys_param_format_error);
            }


            try
            {
                var result = _categoryRespository.GetAllCategory();

                cate = result.FirstOrDefault(x => x.ID.Equals(ID));
                if (cate == null)
                {

                    return Result(cate, Domain.ErrorCode.cate_get_fail);
                }
                else
                {

                    cate.Url = string.Format(@"http://{0}/Model/GetModelList?keyword={1}&name={2}", GlobalData.Instance.Authority, cate.Keywords ?? "", cate.Name ?? "");
                    return Result(cate);
                }

            
            }
            catch (Exception ex)
            {

                LogHelper.WriteException("GetCategoryByID(string ID)", ex);
                return Result(cate, Domain.ErrorCode.sys_fail);
            }

           
           
            

           // throw new NotImplementedException();
        }


        public Domain.XCResult<List<Category>> GetCategoryList(string keywords)
        {


            List<Category> category = new List<Category>();

            try
            {


                if (string.IsNullOrEmpty(keywords))
                {
                    return Result(category, Domain.ErrorCode.sys_param_format_error);
                }



               var result =   _categoryRespository.GetAllCategory();



               category = result.Where(x => x.Name.Contains(keywords) || (!string.IsNullOrEmpty(x.Keywords) && x.Keywords.Contains(keywords))).ToList();


               category = category.Select(x =>
               {



                   //var url = ;

                   return new Category
                   {
                       ID = x.ID,
                       CateType = x.CateType,
                       Book = x.Book,
                       CateNum = x.CateNum,
                       CreateTime = x.CreateTime,
                       Flag = x.Flag,
                       Keywords = x.Keywords,
                       Level = x.Level,
                       Name = x.Name,
                       ParentID = x.ParentID,
                       UpdateTime = x.UpdateTime,
                       IsShowThumb = x.IsShowThumb,
                       Url = string.Format(@"http://{0}/Model/GetModelList?keyword={1}&name={2}", GlobalData.Instance.Authority, x.Keywords ?? "", x.Name ?? "")


                   };

               }).ToList();

               return Result(category);


            }
            catch (Exception ex)
            {
                LogHelper.WriteException("", ex);

                return Result(category);
            }

           // throw new NotImplementedException();
        }


        public XCResult<Category> GetTopCate(string name)
        {

            Category division = null;


            try
            {
                var allcates = GetCategoryListByCache();

                var cuteName = allcates.FirstOrDefault(x => x.Name.Equals(name));

                if (cuteName != null)
                {
                    var divi = GetLevelCategory(cuteName.ID);

                    return Result(divi);
                }
                else
                {
                    return Result(division);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteException("", ex);
            }


            return Result(division);
        }


        /// <summary>
        /// 获取单位
        /// </summary>
        /// <param name="divisionID"></param>
        /// <returns></returns>
        protected Category GetLevelCategory(string divisionID)
        {
            Category division = null;
            division = GetCategoryListByCache().FirstOrDefault(x => x.ID == divisionID);

            if (division != null && division.ParentID != "0")
            {
                division = GetLevelCategory(division.ParentID);
            }
            return division;
        }


        /// <summary>
        /// 设置缓存：书籍目录
        /// </summary>
        /// <returns></returns>
        public List<Category> GetCategoryListByCache()
        {
           // var appID = Client == null ? "" : Client.TokenInfo.AppID;
            var key = XCCacheHelper.BuildCacheKey(string.Format("Category{0}", ""));
            return XCCacheHelper.Get(key, () =>
            {

            

              return  GetAllCategory().Result;

               // return GetAttendTypeList().Result;
            });
        }
        /// <summary>
        /// 清除缓存：书籍目录
        /// </summary>
        public void FlushCachedCategoryList()
        {
           // var appID = Client == null ? "" : Client.TokenInfo.AppID;
            var key = XCCacheHelper.BuildCacheKey(string.Format("Category{0}", ""));
            XCCacheHelper.RemoveAt(key);
        }

    }
}
