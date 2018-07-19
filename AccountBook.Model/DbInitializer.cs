using AccountBook.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountBook.Model
{
    public class DbInitializer
    {
        public static void Initialize(AppliactionDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.Users.Any())
            {
                return;
            }
            var users = new UserInfo[]
            {
                new UserInfo(){ UserName="mhg",Password="123456",CreateTime=DateTime.Now,IP=Net.Ip,RoleName="普通用户"},
                 new UserInfo(){ UserName="张三",Password="123456",CreateTime=DateTime.Now,IP=Net.Ip,RoleName="普通用户"},
                  new UserInfo(){ UserName="李思",Password="123456",CreateTime=DateTime.Now,IP=Net.Ip,RoleName="普通用户"},
                   new UserInfo(){ UserName="老王",Password="123456",CreateTime=DateTime.Now,IP=Net.Ip,RoleName="普通用户"},
            };
            foreach (UserInfo s in users)
            {
                context.Users.Add(s);
            }
            context.SaveChanges();

            var expense = new Expense[] {
                new Expense{MorningMoney=3,AfternoonMoney=14,EveningMoney=10,MoreMoney=1,CreateTime=DateTime.Now,Remark="消费么" },
                 new Expense{MorningMoney=4,AfternoonMoney=14,EveningMoney=10,MoreMoney=1,CreateTime=DateTime.Now,Remark="消费么" },
                  new Expense{MorningMoney=5,AfternoonMoney=14,EveningMoney=10,MoreMoney=1,CreateTime=DateTime.Now,Remark="消费么" },
                   new Expense{MorningMoney=6,AfternoonMoney=14,EveningMoney=10,MoreMoney=1,CreateTime=DateTime.Now,Remark="消费么" },
                    new Expense{MorningMoney=7,AfternoonMoney=14,EveningMoney=10,MoreMoney=1,CreateTime=DateTime.Now,Remark="消费么" },
            };
            foreach (var item in expense)
            {
                context.Expense.Add(item);
            }
            context.SaveChanges();
        }
    }
}
