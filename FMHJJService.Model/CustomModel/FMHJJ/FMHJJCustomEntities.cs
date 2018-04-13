using FMHJJService.Model.FMHJJ;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMHJJService.Model.CustomModel.FMHJJ
{
    public class FMHJJCustomEntities : DbContext
    {
        public FMHJJCustomEntities() : base("name=FMHJJCustomEntities")
        {            
        }

        public DbSet<ProductInfoDetailModel> ProductInfoDetailModel { get; set; }
        public DbSet<ProductInfoModel> ProductInfoModel { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductInfoDetailModel>().ToTable("Base_ProductInfo_Detail");
            modelBuilder.Entity<ProductInfoModel>().ToTable("Base_ProductInfo");
        }
    }
}
