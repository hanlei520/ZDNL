using DataModel;
using PropertyManagesSystem.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PropertyManagesSystem.Controllers
{
    [MyActionFilter]
    public class IndexController : BaseController
    {
        // GET: Index
        public ActionResult Index()
        {
            //查询房屋信息，并放到viewbag中
            List<HouseInfo> list = HouseInfoService.GetAllHouseList();
            if (list!=null)
            {
                ViewBag.Hous = list.Count;
            }
            List<UserInfo> userInfos = UserInfoService.GetUserInfo();
            if (userInfos!=null)
            {
                ViewBag.UserInfo = userInfos.Count;
            }           
            List<ComplaInfo> complaInfos = ComplaInfoService.GetAllComplaInfo();
            if (complaInfos!=null)
            {
                ViewBag.ComplaList = complaInfos.Count;
            }        
            List<RepairInfo> repairInfoList = RepairInfoService.GetAllRepairInfo();
            if (repairInfoList!=null)
            {
                ViewBag.RepairList = repairInfoList.Count;
            }                      
            return View();
        }
    }
}