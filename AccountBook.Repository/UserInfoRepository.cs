using AccountBook.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountBook.Repository
{
  public  class UserInfoRepository
    {
        public List<UserInfo> GetList()
        {
            string sql = "select *  from users";
            using (var conn=ConnectionFactory.OpenConnection())
            {
                return conn.Query<UserInfo>(sql).ToList();
            }
        }
    }
}
