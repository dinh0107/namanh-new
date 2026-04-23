using System;
using System.ComponentModel.DataAnnotations;

namespace namanh.Models
{
    public class Article
    {
        public int Id { get; set; }
        [Display(Name = "Tiêu đề", Description = "Tiêu đề dài tối đa 150 ký tự"), Required(ErrorMessage = "Hãy nhập tiêu đề"), StringLength(150, ErrorMessage = "Tối đa 150 ký tự"), UIHint("TextBox")]
        public string Subject { get; set; }
        [Display(Name = "Trích dẫn ngắn"), Required(ErrorMessage = "Hãy nhập trích dẫn ngắn"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string Description { get; set; }
        [Display(Name = "Nội dung"), UIHint("EditorBox")]
        public string Body { get; set; }
        [Display(Name = "Ảnh đại diện"), UIHint("ImageArticle")]
        public string Image { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [Display(Name = "Ngày đăng")]
        public DateTime CreateDate { get; set; }
        public bool Draft { get; set; } = false;
        public int View { get; set; }
        [Display(Name = "Danh mục bài viết"), Required(ErrorMessage = "Hãy chọn danh mục bài viết")]
        public int ArticleCategoryId { get; set; }

        [Display(Name = "Dịch vụ liên kết")]
        public int? CarServiceId { get; set; }

        [Display(Name = "Hoạt động")]
        public bool Active { get; set; }
        [Display(Name = "Hiển thị menu")]
        public bool ShowMenu { get; set; }
        [Display(Name = "Hiện trang chủ")]
        public bool Home { get; set; }
        [Display(Name = "Đường dẫn"), StringLength(300), UIHint("TextBox")]
        public string Url { get; set; }
        [Display(Name = "Thẻ tiêu đề"), StringLength(100, ErrorMessage = "Tối đa 100 ký tự"), UIHint("TextBox")]
        public string TitleMeta { get; set; }
        [Display(Name = "Thẻ mô tả"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string DescriptionMeta { get; set; }

        public virtual ArticleCategory ArticleCategory { get; set; }


        public virtual CarService CarService { get; set; }


        public Article()
        {
            CreateDate = DateTime.Now;
            View = 1;
        }
    }
}
