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
   public class MenuManagerApp:BaseApp<Menu>
    {




        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="org">The org.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="System.Exception">未能找到该组织的父节点信息</exception>
        public string Add(Menu org)
        {
         //   ChangeModuleCascade(org);

            Repository.Add(org);
            UnitWork.Save();
            return org.Id;
        }

        public string Update(Menu org)
        {
            //ChangeModuleCascade(org);

            ////获取旧的的CascadeId
            //var cascadeId = Repository.FindSingle(o => o.Id == org.Id).CascadeId;
            ////根据CascadeId查询子部门
            //var orgs = Repository.Find(u => u.CascadeId.Contains(cascadeId) && u.Id != org.Id)
            //.OrderBy(u => u.CascadeId).ToList();

            //更新操作
            UnitWork.Update(org);
            UnitWork.Save();
            //更新子部门的CascadeId
            //foreach (var a in orgs)
            //{
            //    ChangeModuleCascade(a);
            //    UnitWork.Update(a);
            //}

            return org.Id;
        }

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


        /// <summary>
        /// 加载所有导航菜单
        /// TODO:获取所有导航菜单
        /// </summary>
        /// <param name="userId">The user unique identifier.</param>
        public List<Menu> LoadMenus()
        {
            ////用户角色
            //var userRoleIds =
            //    UnitWork.Find<Relevance>(u => u.FirstId == userId && u.Key == Define.USERROLE).Select(u => u.SecondId).ToList();

            ////用户角色与自己分配到的角色ID
            //var moduleIds =
            //    UnitWork.Find<Relevance>(
            //        u =>
            //            (u.FirstId == userId && u.Key == Define.USERORG) ||
            //            (u.Key == Define.ROLEORG && userRoleIds.Contains(u.FirstId))).Select(u => u.SecondId).ToList();

            //if (!moduleIds.Any()) return new List<Org>();
            return UnitWork.Find<Menu>(null).ToList();
        }






        public TableData Load(QueryFlowSchemeListReq request)
        {
            return new TableData
            {
                count = Repository.GetCount(null),
                data = Repository.Find(request.page, request.limit, "CreateTime desc").ToList()
            };
        }


        /// <summary>
        /// 加载特定角色的角色
        /// </summary>
        /// <param name="roleId">The role unique identifier.</param>
        public List<Org> LoadForRole(string roleId)
        {
            var moduleIds =
                UnitWork.Find<Relevance>(u => u.FirstId == roleId && u.Key == Define.ROLEORG)
                    .Select(u => u.SecondId)
                    .ToList();
            if (!moduleIds.Any()) return new List<Org>();
            return UnitWork.Find<Org>(u => moduleIds.Contains(u.Id)).ToList();
        }
    }
}
