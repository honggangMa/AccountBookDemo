using System;
using System.Collections.Generic;
using System.Text;

namespace AccountBook.Model
{
   public class Expense
    {
        public int Id { get; set; }
        public  virtual UserInfo User { get; set; }
    }
}
