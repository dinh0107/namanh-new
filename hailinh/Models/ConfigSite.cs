using System.ComponentModel.DataAnnotations;

namespace hailinh.Models
{
    public class ConfigSite
    {
        public int Id { get; set; }
        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Facebook"), Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string Facebook { get; set; }
        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Youtube"), Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string Youtube { get; set; }
        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Twitter"), Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string Twitter { get; set; }
        [StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), Display(Name = "Đường dẫn Instagram"), Url(ErrorMessage = "Đường dẫn không chính xác"), UIHint("TextBox")]
        public string Instagram { get; set; }
        [StringLength(500, ErrorMessage = "Tối đa 4000 ký tự"), Display(Name = "Mã nhúng Messenger"), UIHint("TextBox")]
        public string Messenger { get; set; }
        [Display(Name = "Logo"), UIHint("ImageConfig")]
        public string Image { get; set; }
        [Display(Name = "Ảnh chia sẻ"), UIHint("ImageConfig")]
        public string ImageShare { get; set; }

        [Display(Name = "Xe 4 chỗ"), UIHint("ImageConfig")]
        public string Car4 { get; set; }
        [Display(Name = "Xe 7 chỗ"), UIHint("ImageConfig")]
        public string Car7 { get; set; }
        [Display(Name = "Xe 16 chỗ"), UIHint("ImageConfig")]
        public string Car16 { get; set; }

        [Display(Name = "Favicon"), UIHint("ImageConfig")]
        public string Favicon { get; set; }
        [StringLength(4000, ErrorMessage = "Tối đa 4000 ký tự"), Display(Name = "Mã Google Map"), UIHint("TextArea")]
        public string GoogleMap { get; set; }
        [StringLength(4000, ErrorMessage = "Tối đa 4000 ký tự"), Display(Name = "Mã Google Analytics"), UIHint("TextArea")]
        public string GoogleAnalytics { get; set; }
        [Display(Name = "Địa chỉ"), UIHint("TextBox")]
        public string Place { get; set; }
        [Display(Name = "Thẻ title"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), UIHint("TextBox")]
        public string Title { get; set; }
        [Display(Name = "Ảnh giới thiệu"), UIHint("ImageAbout")]
        public string AboutImage { get; set; }
        [Display(Name = "Ảnh Sứ mệnh"), UIHint("ImageAbout")]
        public string AboutMission { get; set; }
        [Display(Name = "Thông tin bài giới thiệu"), UIHint("EditorBox")]
        public string AboutText { get; set; }
        [Display(Name = "Đường dẫn Bài giới thiệu"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextBox")]
        public string AboutUrl { get; set; }
        [Display(Name = "Thẻ description"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string Description { get; set; }
        [Display(Name = "Hotline"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), UIHint("TextBox")]
        public string Hotline { get; set; }
        [Display(Name = "Hotline 2"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), UIHint("TextBox")]
        public string Hotline2 { get; set; }
        [Display(Name = "Tài khoản Zalo"), StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), UIHint("TextBox")]
        public string Zalo { get; set; }
        [StringLength(50, ErrorMessage = "Tối đa 50 ký tự"), Display(Name = "Email"), UIHint("TextBox")]
        public string Email { get; set; }
        [Display(Name = "Thông tin liên hệ"), UIHint("EditorBox")]
        public string InfoContact { get; set; }
        [Display(Name = "Thông tin chân trang"), UIHint("EditorBox")]
        public string InfoFooter { get; set; }
        [Display(Name = "Bảng giá"), UIHint("EditorBox")]
        public string Price { get; set; }

        [Display(Name = "Ảnh cuối trang"), UIHint("ImageAbout")]
        public string FooterImage { get; set; }

        // Tiêu đề các mục trang chủ
        [Display(Name = "Tiêu đề mục Xe"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), UIHint("TextBox")]
        public string HomeFleetTitle { get; set; }
        [Display(Name = "Mô tả mục Xe"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string HomeFleetDesc { get; set; }
        [Display(Name = "Tiêu đề mục Bảng giá"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), UIHint("TextBox")]
        public string HomePriceTitle { get; set; }
        [Display(Name = "Mô tả mục Bảng giá"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string HomePriceDesc { get; set; }
        [Display(Name = "Tiêu đề ghi chú bảng giá"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), UIHint("TextBox")]
        public string HomePriceNoteTitle { get; set; }
        [Display(Name = "Tiêu đề CTA báo giá"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), UIHint("TextBox")]
        public string HomeCtaTitle { get; set; }
        [Display(Name = "Mô tả CTA báo giá"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string HomeCtaDesc { get; set; }
        [Display(Name = "Tiêu đề mục Đánh giá"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), UIHint("TextBox")]
        public string HomeReviewTitle { get; set; }
        [Display(Name = "Tiêu đề mục Tin tức"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), UIHint("TextBox")]
        public string HomeNewsTitle { get; set; }
        [Display(Name = "Mô tả mục Tin tức"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string HomeNewsDesc { get; set; }
        [Display(Name = "Tiêu đề mục FAQ"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), UIHint("TextBox")]
        public string HomeFaqTitle { get; set; }
        [Display(Name = "Tiêu đề CTA FAQ"), StringLength(200, ErrorMessage = "Tối đa 200 ký tự"), UIHint("TextBox")]
        public string HomeFaqCtaTitle { get; set; }
        [Display(Name = "Mô tả CTA FAQ"), StringLength(500, ErrorMessage = "Tối đa 500 ký tự"), UIHint("TextArea")]
        public string HomeFaqCtaDesc { get; set; }
    }
}
