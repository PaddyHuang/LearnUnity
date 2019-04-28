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
   public class DisplayModuleManagerApp : BaseApp<DisplayModule>
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
            }
        }
        public void Add(DisplayModule Application)
        {
            if (string.IsNullOrEmpty(Application.Id))
            {
                Application.Id = Guid.NewGuid().ToString();
            }
            Repository.Add(Application);
        }

        public void Update(DisplayModule Application)
        {
            Repository.Update(u => u.Id, Application);
        }


        public List<DisplayModule> GetList(Request.QueryAppListReq request)
        {

            if (request != null && !string.IsNullOrEmpty(request.MenuID))
            {
                var applications = UnitWork.Find<DisplayModule>(x => request.MenuID.Equals(x.BeLongToID) && x.AbroadOrLearn == request.Type);
                return applications.ToList();
            }
            else if (request != null && string.IsNullOrEmpty(request.MenuID))
            {
                var applications = UnitWork.Find<DisplayModule>(x => string.IsNullOrEmpty(x.BeLongToID) && x.AbroadOrLearn == request.Type);
                return applications.ToList();
            }
            else
            {
                var applications = UnitWork.Find<DisplayModule>(null);
                return applications.ToList();
            }

         

            
        }

        #region //1.0 加载数据表格
        public TableData Load(QueryFlowSchemeListReq request)
        {

            var content =  Repository.Find(request.page, request.limit, "CreateTime desc").ToList();

            return new TableData
            {
                count = Repository.GetCount(null),
                data = Repository.Find(request.page, request.limit, "CreateTime desc").ToList()
            };
        }
        #endregion

    }
}
