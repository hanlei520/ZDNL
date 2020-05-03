using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DAL
{
    /// <summary>
    /// 工厂类
    /// </summary>
  public  class DALFactory
    {
        //简单工厂设计模式创建对象,属性方式，只读
        public static AdminInfoDAL AdminInfoDAL { get { return new AdminInfoDAL(); } }

        public static ComplaInfoDAL ComplaInfoDAL { get { return new ComplaInfoDAL(); } }

        public static CostInfoDAL CostInfoDAL { get { return new CostInfoDAL(); } }
        public static FloorInfoDAL FloorInfoDAL { get { return new FloorInfoDAL(); } }
        public static HouseInfoDAL HouseInfoDAL { get { return new HouseInfoDAL(); } }
        public static PropertyInfoDAL PropertyInfoDAL { get { return new PropertyInfoDAL(); } }
        public static R_UserInfo_RoleInfoDAL R_UserInfo_RoleInfoDAL { get { return new R_UserInfo_RoleInfoDAL(); } }

        public static RepairInfoDAL RepairInfoDAL { get { return new RepairInfoDAL(); } }
        public static RoleInfoDAL RoleInfoDAL { get { return new RoleInfoDAL(); } }
        public static UserInfoDAL UserInfoDAL { get { return new UserInfoDAL(); } }
        public static VillageInfoDAL VillageInfoDAL { get { return new VillageInfoDAL(); } }
        public static UnitOfWork UnitOfWork { get { return new UnitOfWork(); } }
    }
}
