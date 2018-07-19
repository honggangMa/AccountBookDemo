using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AccountBook.Model
{
  public  class Log
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }   
        [MaxLength(100)]
        public string  IP { get; set; }
        public bool State { get; set; }
        public string  Content { get; set; }
        [MaxLength(100)]
        public string  Address { get; set; }
        public virtual UserInfo User { get; set; }
    }
}
