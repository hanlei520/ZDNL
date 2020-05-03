using AutoMapper;
using DataModel;
using EntityModel.AdminInfoDTO;
using EntityModel.ComplaInfoDTO;
using EntityModel.CostInfoDTO;
using EntityModel.FloorInfoDTO;
using EntityModel.HouseInfoDTO;
using EntityModel.PropertyInfoDTO;
using EntityModel.RepairInfoDTO;
using EntityModel.RoleInfoDTO;
using EntityModel.UserInfoDTO;
using EntityModel.VillageInfoDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    /// <summary>
    /// 用于配置映射关系
    /// </summary>
    public class AutoMapperProfile : Profile//必须继承Profile
    {
        public AutoMapperProfile()
        {
            //配置映射关系，可以双向映射,后续优化可以使用反射自动注册    

            CreateMap<HouseInfoInputDTO, HouseInfo>();
            CreateMap<HouseInfoOutputDTO, HouseInfo>();

            CreateMap<FloorInfoInputDTO, FloorInfo>();

            CreateMap<VillageInfoInputDTO, VillageInfo>();

            CreateMap<UserInfoInputDTO, UserInfo>();

            CreateMap<ComplaInfoInputDTO, ComplaInfo>();
            CreateMap<ComplaInfoUpdateInputDTO, ComplaInfo>();

            CreateMap<RepairInfoInputDTO, RepairInfo>();

            CreateMap<CostInfoInputDTO, CostInfo>();
            CreateMap<CostInfoUpdateInputDTO, CostInfo>();

            CreateMap<PropertyInfoInputDTO, PropertyInfo>();

            CreateMap<AdminInfoInputDTO, AdminInfo>();

            CreateMap<RoleInfoInputDTO, RoleInfo>();

            // 使用CreateMap ...等等。这里（配置文件方法与配置方法相同）
        }
    }
}
