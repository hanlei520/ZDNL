namespace DataModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EFModel : DbContext
    {
        public EFModel()
            : base("name=EFModel")
        {
        }

        public virtual DbSet<AdminInfo> AdminInfo { get; set; }
        public virtual DbSet<ComplaInfo> ComplaInfo { get; set; }
        public virtual DbSet<CostInfo> CostInfo { get; set; }
        public virtual DbSet<FloorInfo> FloorInfo { get; set; }
        public virtual DbSet<HouseInfo> HouseInfo { get; set; }
        public virtual DbSet<PropertyInfo> PropertyInfo { get; set; }
        public virtual DbSet<R_UserInfo_RoleInfo> R_UserInfo_RoleInfo { get; set; }
        public virtual DbSet<RepairInfo> RepairInfo { get; set; }
        public virtual DbSet<RoleInfo> RoleInfo { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<VillageInfo> VillageInfo { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminInfo>()
                .Property(e => e.PassWord)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CostInfo>()
                .Property(e => e.WaterMeter)
                .HasPrecision(6, 2);

            modelBuilder.Entity<CostInfo>()
                .Property(e => e.ElectricMete)
                .HasPrecision(6, 2);

            modelBuilder.Entity<CostInfo>()
                .Property(e => e.WaterPrice)
                .HasPrecision(6, 2);

            modelBuilder.Entity<CostInfo>()
                .Property(e => e.WirePrice)
                .HasPrecision(6, 2);

            modelBuilder.Entity<CostInfo>()
                .Property(e => e.SumMoney)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PropertyInfo>()
                .Property(e => e.PropertyCost)
                .HasPrecision(6, 2);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.PassWord)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
