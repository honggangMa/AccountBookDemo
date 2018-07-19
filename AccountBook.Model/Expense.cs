using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AccountBook.Model
{
   public class Expense
    {
        public int Id { get; set; }
        public decimal MorningMoney { get; set; }
        public decimal AfternoonMoney { get; set; }
        public decimal EveningMoney { get; set; }
        /// <summary>
        /// 零碎花费
        /// </summary>
        public decimal MoreMoney { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        [MaxLength(500)]
        public string  Remark { get; set; }
        public  virtual UserInfo User { get; set; }
    }
}
