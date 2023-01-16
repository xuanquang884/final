using Microsoft.EntityFrameworkCore;
using OrderFoodApi_DXC.Data;

namespace OrderFoodApi_DXC.Api.Data
{
    public class OrderFoodDBContext : DbContext
    {
        public OrderFoodDBContext(DbContextOptions<OrderFoodDBContext> otp) : base(otp)
        { }
        #region DbSet
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodItem>();
            modelBuilder.Entity<UserInfo>(entity => 
            { 
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.Property(e => e.HoTen).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(50);
            });
                
        }
    }
}
