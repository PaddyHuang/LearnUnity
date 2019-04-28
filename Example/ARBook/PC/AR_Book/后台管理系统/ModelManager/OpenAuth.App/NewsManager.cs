
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
   public class NewsManager : BaseApp<News>
    {



        /// <summary>
        /// 删除指定ID的部门及其所有子部门
        /// </summary>
        public void DelOrg(string[] ids)
        {
            var delOrg = Repository.Find(u => ids.Contains(u.Id)).ToList();
            foreach (var org in delOrg)
            {

                Repository.Delete(org);

                // Repository.Delete(u => u.CascadeId.Contains(org.CascadeId));
            }
        }
        public void Add(News Application)
        {
            if (string.IsNullOrEmpty(Application.Id))
            {
                Application.Id = Guid.NewGuid().ToString();
            }
            Repository.Add(Application);
        }

        public void Update(News Application)
        {
            Repository.Update(u => u.Id, Application);
        }


        public List<News> GetList(Request.QueryAppListReq request)
        {

            if (request == null)
            {
                var applications = UnitWork.Find<News>(null);

                return applications.ToList();
            }
            else
            {
                var applications = UnitWork.Find<News>(x=>x.BelongToID.Equals(request.MenuID));

                return applications.ToList();
            }


            
        }

        #region //1.0 加载数据表格

        public TableData Load(QueryFlowSchemeListReq request)
        {
            return new TableData
            {
                count = Repository.GetCount(null),
                data = Repository.Find(request.page, request.limit, "CreateTime desc")
            };
        }
        #endregion
    }
}
