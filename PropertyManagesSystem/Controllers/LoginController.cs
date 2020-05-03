using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel;
using IService;
using Service;

namespace PropertyManagesSystem.Controllers
{
    public class LoginController : Controller
    {
        public IUserInfoService userInfoService { get; set; }
        public IAdminInfoService adminInfoService { get; set; }
        public IRoleInfoService roleInfoService { get; set; }
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        //业主登录
        public string UserLogin(string userName, string userPassWord)
        {
            //最好先做非等空验证等
            UserInfo inputEntity = new UserInfo
            {
                UserID = userName,
                PassWord = userPassWord,
            };
            UserInfo model = userInfoService.UserLogin(inputEntity);

            if (model != null)
            {
                //session保存数据
                Session["UserID"] = model.UserID;
                Session["userName"] = model.Name;
                Session["HousID"] = model.HousID;


                return "ok";
            }
            else
            {

                return "no";
            }
        }

        //管理员登录
        public string AdminLogin(string userName, string userPassWord)
        {
            //最好先做非等空验证等
            AdminInfo inputEntity = new AdminInfo
            {
                AdminID = userName,
                PassWord = userPassWord,
            };
            AdminInfo model = adminInfoService.Login(inputEntity);

            if (model != null)
            {
                //session保存数据
                Session["AdminID"] = model.AdminID;
                Session["userName"] = model.Name;
                Session["AdminRoleID"] = roleInfoService.GetRoleID(model.AdminID);//保存用户拥有的所有角色字符串  2,5
                return "ok";
            }
            else
            {
                return "no";
            }
        }
        //退出登录
        public ActionResult ReLogin()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");//重新登录
        }
    }
}