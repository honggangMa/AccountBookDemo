using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Dapper;
namespace AccountBook.Repository
{
   public class ConnectionFactory
    {        
        public static string ConnectionStr { get; set; }
        public static IDbConnection OpenConnection()
        {
            if (!string.IsNullOrEmpty(ConnectionStr))
            {
                IDbConnection conn = new SqlConnection(ConnectionStr);
                conn.Open();
                return conn;
            }
            else { throw new ApplicationException("数据库连接字符串是空的"); }
        }
    }
}
