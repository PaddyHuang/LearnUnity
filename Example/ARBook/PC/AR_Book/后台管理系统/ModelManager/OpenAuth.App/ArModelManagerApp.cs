
using Infrastructure;
using OpenAuth.App.Request;
using OpenAuth.App.Response;
using OpenAuth.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.App
{
   public class ArModelManagerApp : BaseApp<ARModel>
    {





        /// <summary>
        /// 添加课本
        /// </summary>
        /// <param name="Application"></param>
        public void Add(ARModel Application)
        {
            if (string.IsNullOrEmpty(Application.Id))
            {
                Application.Id = Guid.NewGuid().ToString();
            }
            Repository.Add(Application);
        }
        /// <summary>
        /// 更新课本
        /// </summary>
        /// <param name="Application"></param>
        public void Update(ARModel Application)
        {
            Repository.Update(u => u.Id, Application);
        }

        /// <summary>
        /// 获取课本集合
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<ARModel> GetList(QueryAppListReq request)
        {
            var applications = Repository.Find(null);


            if (request != null)
            {
                if (!string.IsNullOrEmpty(request.key))
                {
                    applications = applications.Where(x => request.key.Contains(x.KeyWords));
                }

                if (!string.IsNullOrEmpty(request.MenuID))
                {
                    applications = applications.Where(x => request.MenuID.Contains(x.BookID)); 
                }


                if (request.Type != -1)
                {
                    applications = applications.Where(x => request.Type == x.Type);
                }

                
            }

            return applications.ToList();
        }



        public ARModel GetARModel(string keyword)
        {
            ARModel result = null;



            if (string.IsNullOrEmpty(keyword))
            {
                return result;
            }
            try
            {
              var  resultlist = Repository.Find(x =>!string.IsNullOrEmpty(x.KeyWords) && x.KeyWords.Contains(keyword)).ToList();

                resultlist.ForEach(x =>
                {
                    var keys = x.KeyWords;

                    var arrkey = keys.Split(',').ToList();


                    if (arrkey.Any(y => y.Equals(keyword)))
                    {
                        result = x;

                        return;
                    }
                });


            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message);
            }



            return result;
        }

        /// <summary>
        ///分页获取信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public TableData Load(QueryFlowSchemeListReq request)
        {


            int pagesize = request.limit;
            int pageindex = request.page;

            var list = Repository.Find(null);

            if (request != null)
            {

                if (!string.IsNullOrEmpty(request.key))
                {

                    

                    list = list.Where(x =>!string.IsNullOrEmpty(x.KeyWords)&& x.KeyWords.Contains(request.key));
                }
                if (!string.IsNullOrEmpty(request.orgId))
                {
                    list = list.Where(x => request.orgId.Equals(x.BookID));
                }
                if (request.TypeCode >0)
                {
                    list = list.Where(x => request.TypeCode.Equals(x.Type));
                }
                return new TableData
                {
                    count = list.Count(),
                    data = list.OrderByDescending(x => x.CreateTime).Skip(pagesize * (pageindex - 1)).Take(pagesize).ToList()
                };
            }
          
            return new TableData
            {
                count = Repository.GetCount(null),
                data = Repository.Find(request.page, request.limit, "CreateTime desc").ToList()
            };
        }


    }
}
