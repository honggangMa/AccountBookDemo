using System;
using System.Collections.Generic;
using System.Text;

namespace AccountBook.Model.ViewModel
{
  public  class LogViewModel
    {
        public string  UserName { get; set; }
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
   
        public string IP { get; set; }
        public bool State { get; set; }
        public string Content { get; set; }
       
        public string Address { get; set; }
    }
}
