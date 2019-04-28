using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SystemCode
{
    public enum ErrorCode
    {
        #region 1-999 系统错误码
        [Description("操作成功.")]
        sys_success = 0,
        [Description("操作失败,请联系管理员.")]
        sys_fail = 1,
        [Description("参数值格式有误.")]
        sys_param_format_error = 2,
        [Description("额度已用完.")]
        sys_app_quota_over = 11,
        #endregion

        #region
        [Description("模型添加失败.")]
        model_add_fail = 101,
        [Description("指定模型不存在.")]
        model_noexist_fail = 102,
        [Description(" 模型删除成功,删除临时文件失败.")]
        model_delete_ok_delfile_fail = 103,
        [Description(" 模型删除失败.")]
        model_delete_fail = 104,
        [Description("模型不存在.")]
        model_noexist = 105,
        [Description("用户添加失败")]
        user_add_fail = -106,
        [Description("用户更新失败")]
        user_update_fail = 107,
        [Description("用户删除失败")]
        user_delete_fail = 108,

        [Description("账户密码不一致")]
        user_login_fail = 109,
        [Description("账号已经存在")]
        user_repeat_fail = 110,
        [Description("模型上传失败")]
        upload_fail = 111,
        [Description("模型名称重复")]
        model_name_repeat = 112,

        [Description("AR模型重复")]
        armodel_repeat_fail = 113,
        [Description("AR模型添加失败")]
        armodel_add_fail = 114,
        [Description("AR模型添加为空")]
        armodel_add_artemplateisull_fail = 115,
        [Description("删除多媒体文件失败")]
        media_delete_fail = 116,

        [Description("组添加失败")]
        group_add_fail = 117,
        [Description("组不存在")]
        group_not_exists = 118,
        [Description("组更新失败")]
        group_update_fail = 119,
        [Description("组删除失败")]
        group_delete_fail = 120,
        [Description("组获取失败")]
        group_get_fail = 121,

        [Description("角色添加失败")]
        role_add_fail = 122,
        [Description("角色更新失败")]
        role_update_fail = 123,
        [Description("角色获取失败")]
        role_get_fail = 124,
        [Description("角色名称重复")]
        role_name_repeat_fail,

        [Description("菜单添加失败")]
        menu_add_fail = 125,
        [Description("菜单更新失败")]
        menu_update_fail = 126,
        [Description("菜单删除失败")]
        delete_menu_fail = 127,
        [Description("权限添加失败")]
        permission_add_fail = 128,
        [Description("权限更新失败")]
        permission_update_fail = 129,

        [Description("目录名称不能为空")]
        cate_name_isnull = 130,
        [Description("目录序号不能为空")]
        cate_num_isnull = 131,
        [Description("目录名称重复")]
        cate_name_repeat_fail = 132,
        [Description("添加类目失败")]
        cate_add_fail = 133,
        [Description("删除类目失败")]
        delete_cate_fail = 134,
        [Description("获取类目失败")]
        cate_get_fail = 135,
        [Description("更新类目失败，原因是名称重复")]
        cate_update_name_repeat_fail = 136,
        [Description("添加微信公众号公司名称不为空")]
        app_add_company_name_null_fail = 137,
        [Description("添加微信公众号AppID不为空")]
        app_add_appid_null_fail = 138,
        [Description("添加微信公众号AppSecret不为空")]
        app_add_appsecret_null_fail = 139,
        [Description("添加微信公众号为空")]
        app_add_fail = 140,
        [Description("添加微信公众号已经重复")]
        wechat_repeat_fail = 141,
        [Description("删除微信公众号失败")]
        del_wechat_fail = 142,
        [Description("更新微信公众号失败")]
        wechat_update_fail = 143,
        [Description("公众号中不存在当前粉丝")]
        wx_not_exists_customer = 144,
        [Description("号码重复绑定")]
        phone_bind_repeat,
        [Description("菜单名称重复")]
        menu_name_repeat_fail,
        [Description("菜单不存在")]
        menu_noexist_fail,
        [Description("添加搜索关键字")]
        add_searchkey_log,
        [Description("号码已经被注册")]
        user_phone_used,
        [Description("组名称重复")]
        add_group_name_repeat,



        #endregion

    }
}
