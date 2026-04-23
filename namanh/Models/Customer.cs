using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace namanh.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Display(Name = "Tên khách hàng"), Required(ErrorMessage = "Hãy nhập Tên khách hàng"), StringLength(150, ErrorMessage = "Tối đa 150 ký tự"), UIHint("TextBox")]
        public string Name { get; set; }
        [Display(Name = "Số điện thoại"), Required(ErrorMessage = "Hãy nhập Số điện thoại"), StringLength(10, ErrorMessage = "Tối đa 10 ký tự"), UIHint("TextBox")]
        public string Mobile { get; set; }
        [Display(Name = "Mã khách hàng"), Required, StringLength(50, ErrorMessage = "Tối đa 50 ký tự")]
        public string Code { get; set; }
        public bool Active { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime CreateDate { get; set; }

        public virtual ICollection<Voucher> Vouchers { get; set; }
        public virtual ICollection<Trip> Trips { get; set; }

        public Customer()
        {
            CreateDate = DateTime.Now;
        }
    }
}