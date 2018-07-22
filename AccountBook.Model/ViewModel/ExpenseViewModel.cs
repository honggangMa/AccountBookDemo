using System;
using System.Collections.Generic;
using System.Text;

namespace AccountBook.Model.ViewModel
{
   public class ExpenseViewModel
    {
        public int Id { get; set; }
        public decimal MorningMoney { get; set; }
        public decimal AfternoonMoney { get; set; }
        public decimal EveningMoney { get; set; }      
        public decimal MoreMoney { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }       
        public string Remark { get; set; }
        public string  UserName { get; set; }
        public decimal DaySumMoney { get; set; }
    }
}
