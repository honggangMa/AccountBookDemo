using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountBook.Model
{
  public  class AppliactionDbContext:DbContext
    {
        public AppliactionDbContext(DbContextOptions<AppliactionDbContext> options):base(options)
        {

        }
        public DbSet<UserInfo> Users { get; set; }
        public DbSet<Expense> Expense { get; set; }

    }
}
