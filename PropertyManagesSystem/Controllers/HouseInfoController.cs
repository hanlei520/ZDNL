using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel;
using EntityModel;
using EntityModel.HouseInfoDTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PropertyManagesSystem.Filters;
using Service;

namespace PropertyManagesSystem.Controllers
{
    [MyActionFilter]

    public class HouseInfoController : BaseController
    {
        // GET: HouseInfo
        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        public ActionResult HouseIndex()
        {

            List<VillageInfo> model  = VillageInfoService.GetVillageInfo();
            List<FloorInfo> floorInfos = FloorInfoService.GetFloorInfo();
            ViewBag.Village = model;
            ViewBag.FloorInfo = floorInfos;
            return View();
        }
        /// <summary>
        /// 添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult HouseAdd()
        {
            List<VillageInfo> model = VillageInfoService.GetVillageInfo();
            List<FloorInfo> floorInfos = FloorInfoService.GetFloorInfo();
            ViewBag.Village = model;
            ViewBag.FloorInfo = floorInfos;
            return View();
        }

        /// <summary>
        /// 查询房屋编号是否已存在
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string CheckRepeat(string value)
        {
            if (HouseInfoService.CheckRepeat(value))
            {
                //说明房屋编号信息已存在--告诉前端，不合法
                return "no";
            }
            else
            {
                return "ok";
            }
        }
        /// <summary>
        /// 修改页面
        /// </summary>
        /// <returns></returns>
        public ActionResult HouseUpdate(int ID)
        {
            List<VillageInfo> model = VillageInfoService.GetVillageInfo();
            List<FloorInfo> floorInfos = FloorInfoService.GetFloorInfo();
            List<UserInfo> userInfos = UserInfoService.GetUserInfo();
            ViewBag.Village = model;
            ViewBag.FloorInfo = floorInfos;
            ViewBag.UserInfo = userInfos;

            HouseInfo houseInfo = HouseInfoService.GetHouseInfoByID(ID);
            ViewBag.HouseInfo = houseInfo;
            return View();
        }

        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string Delete(Array id)
        {

            if (HouseInfoService.Delete(id))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }

        /// <summary>
        /// 根据id置空房间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public string Empty(Array id)
        {
            //int i = 0;
            if (HouseInfoService.Empty(id))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }



        #region DTO

        /// <summary>
        /// 添加房屋信息
        /// </summary>
        /// <param name="HouseInfoJson">前端返回的json数据</param>
        /// <returns></returns>
        public string HouseInfoAdd(string HouseInfoJson)
        {
            //添加数据
            //Json转实体类
            HouseInfoInputDTO houseInfo = JsonConvert.DeserializeObject<HouseInfoInputDTO>(HouseInfoJson);
            if (HouseInfoService.DTOAdd(houseInfo))
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }

        /// <summary>
        /// 数据更新
        /// </summary>
        /// <param name="HouseInfoJson">前端返回的JSON数据</param>
        /// <returns></returns>
        public string HouseInfoUpdate(string HouseInfoJson)
        {
            int outID; 
            //Json转实体类
            HouseInfoInputDTO houseInfo =  JsonConvert.DeserializeObject<HouseInfoInputDTO>(HouseInfoJson);
            if (HouseInfoService.DTOUpdate(houseInfo, out outID) > 0)
            {
                return "ok";
            }
            else
            {
                return "no";
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        public string GetHouseInfo()
        {
            List<VillageInfo> model = VillageInfoService.GetVillageInfo();
            List<FloorInfo> floorInfos = FloorInfoService.GetFloorInfo();
            ViewBag.Village = model;
            ViewBag.FloorInfo = floorInfos;
            //分页查询
            //拿到前端传来的数据-->第几页，每页显示多少条
            int pageIndex = Convert.ToInt32(Request["page"]);
            int pageSize = Convert.ToInt32(Request["limit"]);
            //获取文本框的值:有可能为null  ，有可能为"" ，有可能为其他字符串
            string VillageID = Request["VillageID"];
            string selectInfo = Request["selectInfo"];
            //string FloorID = Request["FloorID"];
            string State = Request["State"];

            //设置总条数，默认值为0
            int count = 0;

            //使用json.net序列化数据
            //List<HouseOutput> list = HouseInfoService.GetPageList(pageIndex, pageSize, out count, VillageID,selectInfo, State);

            //DTO
            List<HouseInfoOutputDTO> list = HouseInfoService.DTOGetPageList(pageIndex, pageSize, out count, VillageID, selectInfo, State);
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy'-'MM'-'dd hh':'mm':'ss"//格式化时间，默认是ISO8601格式
            };
            string strjson = JsonConvert.SerializeObject(list, Formatting.Indented, timeConverter);

            //字符串拼接，构造与layui规则相同的json对象数组字符串
            string str = "{\"code\": 0 ,\"msg\": \"\",\"count\": " + count + " ,\"data\":";
            str += strjson + "}";

            return str;
        }
        #endregion
    }
}