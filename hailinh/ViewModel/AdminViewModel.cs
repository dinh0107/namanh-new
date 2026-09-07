using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using hailinh.Models;

namespace hailinh.ViewModel
{
    public class ChangePasswordModel
    {
        [Display(Name = "Mật khẩu hiện tại"), Required(ErrorMessage = "Hãy nhập mật khẩu hiện tại"), UIHint("Password")]
        public string OldPassword { get; set; }
        [Display(Name = "Mật khẩu mới"), Required(ErrorMessage = "Hãy nhập mật khẩu mới"),
         StringLength(16, MinimumLength = 4, ErrorMessage = "Mật khẩu từ 4, 16 ký tự"), UIHint("Password")]
        public string Password { get; set; }
        [Display(Name = "Nhập lại mật khẩu"), System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Nhập lại mật khẩu không chính xác"),
         UIHint("Password")]
        public string ConfirmPassword { get; set; }
    }

    public class AdminLoginModel
    {
        [Display(Name = "Tên đăng nhập"), Required(ErrorMessage = "Hãy nhập tên đăng nhập")]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu"), Required(ErrorMessage = "Hãy nhập mật khẩu")]
        public string Password { get; set; }
    }

    public class InfoAdminViewModel
    {
        public int Articles { get; set; }
        public int Contacts { get; set; }
        public int Admins { get; set; }
        public int Banners { get; set; }
        public int Products { get; set; }
        public int Customers { get; set; }
        public int Trips { get; set; }
        public int TripChanges { get; set; }
        public int Drivers { get; set; }
        public int Expenses { get; set; }
    }

    public class EditAdminViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
        [Display(Name = "Tên đăng nhập"), Required(ErrorMessage = "Bạn chưa điền tên đăng nhập"), StringLength(20, MinimumLength = 4, ErrorMessage = "Tên đăng nhập từ 4 - 20 ký tự"), RegularExpression(@"[a-zA-Z0-9]+$", ErrorMessage = "Chấp nhận chữ cái không dấu, số, viết liền không khoảng trống"), UIHint("TextBox")]
        public string Username { get; set; }
        [Display(Name = "Mật khẩu"), UIHint("Password"), StringLength(20, MinimumLength = 6, ErrorMessage = "Mật khẩu từ 6 - 20 ký tự")]
        public string Password { get; set; }
        public Role Role { get; set; }
    }

    public class ListExpenseViewModel
    {
        [UIHint("DateTimePicker")]
        public string CreateDate { get; set; }
        public int DriverId { get; set; }
        public int Status { get; set; }
        public decimal? Gasoline { get; set; }
        public decimal? ETC { get; set; }
        public decimal? Maintenance { get; set; }
        public decimal? CarWash { get; set; }
        public decimal? Other { get; set; }
        public decimal? Total { get; set; }
        public IPagedList<Expense> Expenses { get; set; }
        public IEnumerable<Driver> Drivers { get; set; }
        public Expenditure Expenditure { get; set; }

    }

    public class InsertExpenseViewModel
    {
        [Display(Name = "Giá"), Required(ErrorMessage = "Hãy nhập Giá"), UIHint("MoneyBox"), DisplayFormat(DataFormatString = "{0:N0}đ")]
        public string Price { get; set; }
        [Display(Name = "Ngày chi"), Required(ErrorMessage = "Hãy chọn ngày chi"), UIHint("DateTimePicker")]
        public string CreateDate { get; set; }
        public IEnumerable<Driver> Drivers { get; set; }
        public Expense Expense { get; set; }
    }

    public static class HtmlEnumHelper
    {
        public static MvcHtmlString ToSelectList<TEnum>(this HtmlHelper html, string name, TEnum selected)
            where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("TEnum phải là enum");

            var items = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString(),
                    Selected = e.Equals(selected)
                });

            return html.DropDownList(name, items);
        }
    }

}
