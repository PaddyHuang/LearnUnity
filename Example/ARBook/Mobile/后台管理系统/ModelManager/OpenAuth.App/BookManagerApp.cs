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

    /// <summary>
    /// /书籍管理类
    /// </summary>
    public class BookManagerApp : BaseApp<Book>
    {
        /// <summary>
        /// 添加课本
        /// </summary>
        /// <param name="Application"></param>
        public void Add(Book Application)
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
        public void Update(Book Application)
        {
            Repository.Update(u => u.Id, Application);
        }

        /// <summary>
        /// 获取课本集合
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<Application> GetList(QueryAppListReq request)
        {
            var applications = UnitWork.Find<Application>(null);

            return applications.ToList();
        }

        /// <summary>
        ///分页获取信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public TableData Load(QueryFlowSchemeListReq request)
        {

            //count = Repository.GetCount(null),

            return new TableData
            {
                count = Repository.GetCount(null),
                data = Repository.Find(request.page, request.limit, "CreateTime desc").ToList()
            };
        }
    }
}
