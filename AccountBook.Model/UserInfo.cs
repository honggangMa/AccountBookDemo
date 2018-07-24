using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AccountBook.Model
{
    public class UserInfo
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(32)]
        public string Password { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public DateTime? UpdateTime { get; set; }
        [MaxLength(80)]
        public string  IP { get; set; }
        [MaxLength(32)]
        public string  RoleName { get; set; }
        public virtual ICollection<Expense> Expense { get; set; }
        public virtual ICollection<Log> Log { get; set; }
    }
}
