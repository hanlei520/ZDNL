using DataModel;
using EntityModel.RoleInfoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
 public   interface IRoleInfoService
    {
        /// <summary>
        /// 根据登录的用户id获取他对应未被禁用的角色字符串
        /// </summary>
        /// <param name="Admin"></param>
        /// <returns></returns>
         string GetRoleID(string AdminID);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="selectInfo"></param>
        /// <returns></returns>
        List<RoleInfo> GetPageList(int pageIndex, int pageSize, out int count, string selectInfo);

        /// <summary>
        ///DTO 添加数据
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
         bool DTOAdd(RoleInfoInputDTO inputEntity);

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
         RoleInfo GetModelByID(int id);


        /// <summary>
        /// 检查是否已存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
         bool CheckRepeat(string value);

        /// <summary>
        /// DTO修改
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <param name="outID"></param>
        /// <returns></returns>
         int DTOUpdate(RoleInfoInputDTO inputEntity, out int outID);

        /// <summary>
        /// 禁用或开启用户--其实就是修改，
        /// </summary>
        /// <param name="id"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        bool ForbidUser(int id, string flag);

        /// <summary>
        /// 获取除了超级管理员的其他角色信息 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
         List<RoleInfo> GetPageList(int pageIndex, int pageSize, out int count);


        /// <summary>
        /// 获取所有角色，先删除再按要求重新添加
        /// </summary>
        /// <param name="arrRoleID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
         bool ConfiRole(string arrRoleID, string AdminID);
    }
}
