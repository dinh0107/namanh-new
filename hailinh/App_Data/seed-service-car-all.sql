-- Seed ALL CarServices with Figma sections (works on any DB)
SET NOCOUNT ON;

-- Refresh copy for every service
UPDATE dbo.CarServices SET
  Slogan = COALESCE(NULLIF(LTRIM(RTRIM(Slogan)), N''), N'Chuyên nghiệp – An toàn – Đúng giờ'),
  Description = COALESCE(NULLIF(LTRIM(RTRIM(Description)), N''),
    N'Dịch vụ thuê xe có lái tại Hà Nội: xe đời mới, tài xế chuyên nghiệp, đón trả tận nơi. Báo giá nhanh trong vài phút.'),
  ImageUrl = COALESCE(NULLIF(LTRIM(RTRIM(ImageUrl)), N''), N'seed/hero-vios.webp'),
  Image = COALESCE(NULLIF(LTRIM(RTRIM(Image)), N''), N'seed/hero-vios.webp');

DELETE FROM dbo.CarServiceDetails;
DELETE FROM dbo.CarServicePrices;

-- Prices for every service
INSERT INTO dbo.CarServicePrices (CarServiceId, RouteDescription, Price, Km, Hot, Sort)
SELECT s.Id, p.RouteDescription, p.Price, NULL, p.Hot, p.Sort
FROM dbo.CarServices s
CROSS JOIN (VALUES
  (N'Hà Nội ⇔ Ninh Bình', N'1.200.000đ', 1, 1),
  (N'Hà Nội ⇔ Hạ Long', N'1.600.000đ', 2, 1),
  (N'Hà Nội ⇔ Hải Phòng', N'1.500.000đ', 3, 0),
  (N'Hà Nội ⇔ Thái Nguyên', N'1.200.000đ', 4, 0),
  (N'Hà Nội ⇔ Bắc Ninh', N'900.000đ', 5, 0),
  (N'Hà Nội ⇔ Hải Dương', N'1.300.000đ', 6, 0),
  (N'Hà Nội ⇔ Nam Định', N'1.400.000đ', 7, 0),
  (N'Hà Nội ⇔ Thanh Hóa', N'2.200.000đ', 8, 0)
) AS p(RouteDescription, Price, Sort, Hot);

-- Included + Why (Highlight=0)
INSERT INTO dbo.CarServiceDetails (CarServiceId, CarServiceType, Name, Desciption, Body, Image, ListImage, Sort)
SELECT s.Id, 0, x.Name, x.Desciption, NULL, NULL, NULL, x.Sort
FROM dbo.CarServices s
CROSS JOIN (VALUES
  (N'Xe đời mới, sạch sẽ', NULL, 1),
  (N'Tài xế chuyên nghiệp, lịch sự', NULL, 2),
  (N'Đón trả tận nơi theo yêu cầu', NULL, 3),
  (N'Hỗ trợ nhiệt tình 24/7', NULL, 4),
  (N'Xe chất lượng cao', N'Đời mới, bảo dưỡng định kỳ', 5),
  (N'Tài xế kinh nghiệm', N'Lái xe an toàn, đúng giờ', 6),
  (N'Giá minh bạch', N'Báo giá rõ ràng, không phát sinh ẩn', 7),
  (N'Hỗ trợ 24/7', N'Luôn sẵn sàng khi bạn cần', 8),
  (N'Đặt xe nhanh', N'Nhận báo giá trong vài phút', 9)
) AS x(Name, Desciption, Sort);

-- Gallery (SupportedCar=1)
INSERT INTO dbo.CarServiceDetails (CarServiceId, CarServiceType, Name, Desciption, Body, Image, ListImage, Sort)
SELECT s.Id, 1, x.Name, NULL, NULL, x.Image, NULL, x.Sort
FROM dbo.CarServices s
CROSS JOIN (VALUES
  (N'Ngoại thất', N'seed/gallery-1.webp', 1),
  (N'Nội thất', N'seed/gallery-2.webp', 2),
  (N'Cốp xe', N'seed/gallery-3.webp', 3)
) AS x(Name, Image, Sort);

-- FAQ (Policy=3)
INSERT INTO dbo.CarServiceDetails (CarServiceId, CarServiceType, Name, Desciption, Body, Image, ListImage, Sort)
SELECT s.Id, 3, x.Name, NULL, x.Body, NULL, NULL, x.Sort
FROM dbo.CarServices s
CROSS JOIN (VALUES
  (N'Giá thuê xe bao gồm những gì?', N'<p>Giá thường bao gồm xăng dầu và lương tài xế. Phí cầu đường, gửi xe, VAT (nếu xuất hóa đơn) sẽ được thông báo trước.</p>', 1),
  (N'Có phát sinh chi phí ẩn không?', N'<p>Không. Mọi khoản phát sinh (nếu có) đều được báo trước và thống nhất với khách.</p>', 2),
  (N'Có xuất hóa đơn VAT không?', N'<p>Có. Quý khách vui lòng cung cấp thông tin xuất hóa đơn khi đặt xe.</p>', 3),
  (N'Đặt xe trước bao lâu?', N'<p>Nên đặt trước 24 giờ. Trường hợp gấp vui lòng gọi hotline để được hỗ trợ ngay.</p>', 4),
  (N'Thanh toán như thế nào?', N'<p>Có thể chuyển khoản hoặc thanh toán tiền mặt với tài xế theo thỏa thuận.</p>', 5)
) AS x(Name, Body, Sort);

-- Testimonial
IF NOT EXISTS (SELECT 1 FROM dbo.Banners WHERE GroupId = 5 AND BannerName = N'Nguyễn Minh Tuấn')
  INSERT INTO dbo.Banners (BannerName, Slogan, Image, Active, GroupId, Url, Sort, Content, ListImage)
  VALUES (N'Nguyễn Minh Tuấn', N'Xe sạch, tài xế đúng giờ và rất lịch sự.', N'seed/avatar.webp', 1, 5, NULL, 1,
          N'Xe sạch, tài xế đúng giờ và rất lịch sự. Đặt xe đi tỉnh rất yên tâm. Sẽ tiếp tục dùng dịch vụ Hà Linh.', NULL);
ELSE
  UPDATE dbo.Banners SET Active=1, Image=COALESCE(NULLIF(Image,N''),N'seed/avatar.webp'),
    Content=N'Xe sạch, tài xế đúng giờ và rất lịch sự. Đặt xe đi tỉnh rất yên tâm. Sẽ tiếp tục dùng dịch vụ Hà Linh.'
  WHERE GroupId=5 AND BannerName=N'Nguyễn Minh Tuấn';

SELECT 'prices' k, COUNT(*) c FROM dbo.CarServicePrices
UNION ALL SELECT 'details', COUNT(*) FROM dbo.CarServiceDetails
UNION ALL SELECT 'services', COUNT(*) FROM dbo.CarServices;
