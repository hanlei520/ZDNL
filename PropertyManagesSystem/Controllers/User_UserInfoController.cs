using DataModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PropertyManagesSystem.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PropertyManagesSystem.Controllers
{
    [MyUserActionFilter]
    public class User_UserInfoController : BaseController
    {
        // GET: User_UserInfo


        /// <summary>
        /// 修改基本信息页面
        /// </summary>
        /// <returns></returns>
        public ActionResult UserAlter()
        {
            string UserID = Session["UserID"].ToString();
            //根据传过来的id获取选中账号的信息,并把查到的数据赋值给viewbag，以便页面绑定数据
            UserInfo model = UserInfoService.GetModelByID(UserID);
            if (model != null)
            {
                ViewBag.model = model;
            }
            return View();
        }
        /// <summary>
        /// 修改密码 视图页面
        /// </summary>
        /// <returns></returns>
        public ActionResult UserResetPwd()
        {
            return View();
        }

        /// <summary>
        /// 基本信息修改
        /// </summary>
        /// <param name="User_UserInfoJson">前端返回的JSON数据</param>
        /// <returns></returns>
        public string UserUpdate(string User_UserInfoJson)
        {
            //添加数据
            UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(User_UserInfoJson);
            if (UserInfoService.UserUpdate(userInfo))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="User_UserInfoJson">前端返回的JSON数据</param>
        /// <returns></returns>
        public string ResetPwd(string User_UserInfoJson)
        {
            //JSON字符串转JSON对象
            JObject jo = (JObject)JsonConvert.DeserializeObject(User_UserInfoJson);
            UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(User_UserInfoJson);
            string UserID = Session["UserID"].ToString();
            if (jo!=null)
            {
                //取值
                string PassWord = jo["PassWord"].ToString();
                string NewPwd = jo["NewPwd"].ToString();
                string Pwd = jo["Pwd"].ToString();
                if (UserInfoService.ResetPwd(PassWord, UserID, NewPwd, Pwd))
                {
                    return "ok";
                }
                else
                {
                    return "no";
                }
            }
            else
            {
                return "no";
            }
           
        }

        /// <summary>
        /// 查询当前密码是否正确 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public string CheckPwd(string value)
        {
            if (UserInfoService.CheckPwd(value))
            {
                //说明密码正确，告诉前端合法
                return "ok";
            }
            else
            {
                return "no";
            }
        }
    }
}