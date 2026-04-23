using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace namanh.Models
{
    public class Driver
    {
        public int Id { get; set; }
        [Display(Name = "Tên lái xe"), Required(ErrorMessage = "Hãy nhập tên Lái xe"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), UIHint("TextBox")]
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }

        public virtual ICollection<Trip> Trips { get; set; }
    }
}