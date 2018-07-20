using Microsoft.EntityFrameworkCore;

namespace AccountBook.Model
{
    public class AppliactionDbContext : DbContext
    {
        public AppliactionDbContext(DbContextOptions<AppliactionDbContext> options) : base(options)
        {
        }
        //卷影属性是指你.NET 实体类中未定义但 EF Core 模型中该实体类型定义  数据注解注入实体
        public DbSet<UserInfo> Users { get; set; }
        public DbSet<Expense> Expense { get; set; }
        public DbSet<Log> Log { get; set; }
        //对 DbContext 指定单数的表名来覆盖默认的表名。fluent api 注册实体
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Course>().ToTable("Course");
        //    modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
        //    modelBuilder.Entity<Student>().ToTable("Student");
        //}
    }
}