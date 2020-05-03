using DataModel;
using EntityModel;
using EntityModel.AdminInfoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
   public interface IAdminInfoService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
        AdminInfo Login(AdminInfo inputEntity);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
         List<AdminOutputDTO> GetPageList(int pageIndex, int pageSize, out int count, string selectInfo);

        /// <summary>
        /// 根据ID删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(Array id);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="AdminID"></param>
        /// <param name="NewPwd"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        bool ResetPwd(string PassWord, string AdminID, string NewPwd, string Pwd);

        /// <summary>
        /// 根据ID重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool RePwd(Array id);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="inpuEntity"></param>
        /// <returns></returns>
        bool DTOAdd(AdminInfoInputDTO inpuEntity);

        /// <summary>
        /// 根据value查询用户编号是否存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool CheckRepeat(string value);


        /// <summary>
        /// 查询当前用户的信息
        /// </summary>
        /// <param name="AdminID"></param>
        /// <returns></returns>
        AdminInfo GetModelByID(string AdminID);

        /// <summary>
        /// 用户信息修改
        /// </summary>
        /// <param name="inputEntity"></param>
        /// <returns></returns>
        int DTOUpdate(AdminInfoInputDTO inputEntity);



        /// <summary>
        /// 禁用或开启用户--其实就是修改，
        /// </summary>
        /// <param name="id"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        bool ForbidUser(int id, string flag);


        /// <summary>
        /// 查询当前密码是否正确
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
         bool CheckPwd(string value);
      
    }
}
