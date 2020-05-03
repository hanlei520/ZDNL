using DataModel;
using EntityModel.UserInfoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
 public   interface IUserInfoService
    {
        /// <summary>
        /// 查询业主所有信息
        /// </summary>
        /// <returns></returns>
         List<UserInfo> GetUserInfo();
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="selectInfo"></param>
        /// <returns></returns>
         List<UserInfo> GetPageList(int pageIndex, int pageSize, out int count, string selectInfo);

        /// <summary>
        /// DTO添加数据
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
         bool DTOAdd(UserInfoInputDTO inputEntity);

        /// <summary>
        /// 根据ID删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         bool Delete(Array id);


        /// <summary>
        /// 根据id获取用户数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserInfo GetUserByID(int id);

        /// <summary>
        /// 业主基本信息的修改查询
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
         UserInfo GetModelByID(string UserID);
        /// <summary>
        /// 根据ID重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         bool RePwd(Array id);
        /// <summary>
        /// 检查用户编号是否已存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
         bool CheckRepeat(string value);

        /// <summary>
        /// 检查用户身份证是否已存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool CheckIDnumber(string value);
        /// <summary>
        /// DTO修改
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="outID"></param>
        /// <returns></returns>
        bool DTOUpdate(UserInfoInputDTO inputEntity, out int outID);

        bool UserUpdate(UserInfo inputEntity);

        /// <summary>
        /// 业主登录
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
         UserInfo UserLogin(UserInfo inputEntity);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="PassWord">当前密码</param>
        /// <param name="UserID">账号</param>
        /// <param name="NewPwd">新密码</param>
        /// <param name="Pwd">确认密码</param>
        /// <returns></returns>
        bool ResetPwd(string PassWord, string UserID, string NewPwd, string Pwd);


        /// <summary>
        /// 查询当前密码是否正确
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
         bool CheckPwd(string value);
    }
}
