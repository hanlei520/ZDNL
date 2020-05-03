using IService;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PropertyManagesSystem.Controllers
{
    public  class BaseController : Controller
    {

        //使用属性的方式注入对象-->只读
        public IAdminInfoService AdminInfoService
        {
            get;set;
        }

        public IRoleInfoService RoleInfoService
        {
            get; set;
        }

        public IHouseInfoService HouseInfoService
        {
            get; set;
        }

        public IVillageInfoService VillageInfoService
        {
            get; set;
        }
        public IFloorInfoService FloorInfoService
        {
            get; set;
        }

        public IUserInfoService UserInfoService
        {
            get; set;
        }
        public ICostInfoService CostInfoService
        {
            get; set;
        }
        public IComplaInfoService ComplaInfoService
        {
            get; set;
        }
        public IRepairInfoService RepairInfoService
        {
            get; set;
        }
        public IPropertyInfoService PropertyInfoService
        {
            get; set;
        }
    }
}