-- Seed full content for CarService pages (Figma sections)
SET NOCOUNT ON;

-- Update hero copy for 4-5 chỗ (template)
UPDATE dbo.CarServices SET
  Slogan = N'Nhỏ gọn – Tiết kiệm – Linh hoạt',
  Description = N'Dịch vụ thuê xe 4 chỗ có lái tại Hà Nội: xe đời mới (Vios, Accent, City), tài xế chuyên nghiệp, đưa đón tận nơi. Phù hợp sân bay, công tác, cưới hỏi và đi tỉnh trong ngày.',
  ImageUrl = COALESCE(NULLIF(ImageUrl, ''), N'seed/hero-vios.webp'),
  Image = COALESCE(NULLIF(Image, ''), N'seed/hero-vios.webp')
WHERE Id = 2;

UPDATE dbo.CarServices SET Slogan = N'Rộng rãi – Êm ái – Gia đình', Description = N'Thuê xe 7 chỗ có lái: Innova, Fortuner, Xpander đời mới, phù hợp gia đình và đi tỉnh.' WHERE Id = 3;
UPDATE dbo.CarServices SET Slogan = N'Đoàn nhóm – Du lịch – Hội nghị', Description = N'Thuê xe 16 chỗ có lái: Transit, Solati đời mới, phù hợp đoàn nhỏ và sự kiện.' WHERE Id = 4;
UPDATE dbo.CarServices SET Slogan = N'Đoàn thể – Sự kiện – Tour', Description = N'Thuê xe 29 chỗ có lái: County, Samco đời mới cho đoàn vừa.' WHERE Id = 5;
UPDATE dbo.CarServices SET Slogan = N'Tour lớn – Hội nghị – Liên tỉnh', Description = N'Thuê xe 45 chỗ có lái: Universe, Hi-Class cho đoàn lớn.' WHERE Id = 6;
UPDATE dbo.CarServices SET Slogan = N'VIP – Đẳng cấp – Riêng tư', Description = N'Thuê xe Limousine VIP có lái: nội thất cao cấp, trải nghiệm riêng tư.' WHERE Id = 7;

-- Clear old detail/price for reseed
DELETE FROM dbo.CarServiceDetails;
DELETE FROM dbo.CarServicePrices;

-- Prices per service (RouteDescription, Price, Sort, Hot, CarServiceId)
;WITH P AS (
  SELECT * FROM (VALUES
    -- 4 chỗ Id=2
    (2, N'Hà Nội ⇔ Ninh Bình', N'1.200.000đ', 1, 1),
    (2, N'Hà Nội ⇔ Hạ Long', N'1.600.000đ', 2, 1),
    (2, N'Hà Nội ⇔ Hải Phòng', N'1.500.000đ', 3, 0),
    (2, N'Hà Nội ⇔ Thái Nguyên', N'1.200.000đ', 4, 0),
    (2, N'Hà Nội ⇔ Bắc Ninh', N'900.000đ', 5, 0),
    (2, N'Hà Nội ⇔ Hải Dương', N'1.300.000đ', 6, 0),
    (2, N'Hà Nội ⇔ Nam Định', N'1.400.000đ', 7, 0),
    (2, N'Hà Nội ⇔ Thanh Hóa', N'2.200.000đ', 8, 0),
    -- 7 chỗ
    (3, N'Hà Nội ⇔ Ninh Bình', N'1.500.000đ', 1, 1),
    (3, N'Hà Nội ⇔ Hạ Long', N'1.900.000đ', 2, 1),
    (3, N'Hà Nội ⇔ Hải Phòng', N'1.800.000đ', 3, 0),
    (3, N'Hà Nội ⇔ Thái Nguyên', N'1.500.000đ', 4, 0),
    (3, N'Hà Nội ⇔ Bắc Ninh', N'1.100.000đ', 5, 0),
    (3, N'Hà Nội ⇔ Thanh Hóa', N'2.600.000đ', 6, 0),
    -- 16 chỗ
    (4, N'Hà Nội ⇔ Ninh Bình', N'2.200.000đ', 1, 1),
    (4, N'Hà Nội ⇔ Hạ Long', N'2.800.000đ', 2, 1),
    (4, N'Hà Nội ⇔ Hải Phòng', N'2.500.000đ', 3, 0),
    (4, N'Hà Nội ⇔ Thái Nguyên', N'2.200.000đ', 4, 0),
    (4, N'Hà Nội ⇔ Thanh Hóa', N'3.500.000đ', 5, 0),
    -- 29 chỗ
    (5, N'Hà Nội ⇔ Ninh Bình', N'3.200.000đ', 1, 1),
    (5, N'Hà Nội ⇔ Hạ Long', N'3.800.000đ', 2, 1),
    (5, N'Hà Nội ⇔ Hải Phòng', N'3.500.000đ', 3, 0),
    (5, N'Hà Nội ⇔ Thanh Hóa', N'4.800.000đ', 4, 0),
    -- 45 chỗ
    (6, N'Hà Nội ⇔ Ninh Bình', N'4.500.000đ', 1, 1),
    (6, N'Hà Nội ⇔ Hạ Long', N'5.200.000đ', 2, 1),
    (6, N'Hà Nội ⇔ Hải Phòng', N'4.800.000đ', 3, 0),
    (6, N'Hà Nội ⇔ Thanh Hóa', N'6.500.000đ', 4, 0),
    -- Limousine
    (7, N'Hà Nội ⇔ Ninh Bình', N'2.800.000đ', 1, 1),
    (7, N'Hà Nội ⇔ Hạ Long', N'3.500.000đ', 2, 1),
    (7, N'Hà Nội ⇔ Hải Phòng', N'3.200.000đ', 3, 0),
    (7, N'Hà Nội ⇔ Nội Bài (1 chiều)', N'800.000đ', 4, 0)
  ) AS t(CarServiceId, RouteDescription, Price, Sort, Hot)
)
INSERT INTO dbo.CarServicePrices (CarServiceId, RouteDescription, Price, Km, Hot, Sort)
SELECT CarServiceId, RouteDescription, Price, NULL, Hot, Sort FROM P;

-- Details: Highlight 0 = included(1-4) + why(5-9); SupportedCar 1 = gallery; Policy 3 = FAQ
;WITH D AS (
  SELECT * FROM (VALUES
    -- Included (Highlight, Sort 1-4) for each service
    (2, 0, N'Xe đời mới, sạch sẽ', NULL, 1),
    (2, 0, N'Tài xế chuyên nghiệp, lịch sự', NULL, 2),
    (2, 0, N'Đón trả tận nơi theo yêu cầu', NULL, 3),
    (2, 0, N'Hỗ trợ nhiệt tình 24/7', NULL, 4),
    (2, 0, N'Xe chất lượng cao', N'Đời mới, bảo dưỡng định kỳ', 5),
    (2, 0, N'Tài xế kinh nghiệm', N'Lái xe an toàn, đúng giờ', 6),
    (2, 0, N'Giá minh bạch', N'Báo giá rõ ràng, không phát sinh ẩn', 7),
    (2, 0, N'Hỗ trợ 24/7', N'Luôn sẵn sàng khi bạn cần', 8),
    (2, 0, N'Đặt xe nhanh', N'Nhận báo giá trong vài phút', 9),
    -- Gallery
    (2, 1, N'Ngoại thất', NULL, 1),
    (2, 1, N'Nội thất', NULL, 2),
    (2, 1, N'Cốp xe', NULL, 3),
    -- FAQ
    (2, 3, N'Giá thuê xe bao gồm những gì?', N'<p>Giá thường bao gồm xăng dầu và lương tài xế. Phí cầu đường, gửi xe, VAT (nếu xuất hóa đơn) sẽ được thông báo trước.</p>', 1),
    (2, 3, N'Có phát sinh chi phí ẩn không?', N'<p>Không. Mọi khoản phát sinh (nếu có) đều được báo trước và thống nhất với khách.</p>', 2),
    (2, 3, N'Có xuất hóa đơn VAT không?', N'<p>Có. Quý khách vui lòng cung cấp thông tin xuất hóa đơn khi đặt xe.</p>', 3),
    (2, 3, N'Đặt xe trước bao lâu?', N'<p>Nên đặt trước 24 giờ. Trường hợp gấp vui lòng gọi hotline để được hỗ trợ ngay.</p>', 4),
    (2, 3, N'Thanh toán như thế nào?', N'<p>Có thể chuyển khoản hoặc thanh toán tiền mặt với tài xế theo thỏa thuận.</p>', 5)
  ) AS t(CarServiceId, CarServiceType, Name, Body, Sort)
)
INSERT INTO dbo.CarServiceDetails (CarServiceId, CarServiceType, Name, Desciption, Body, Image, ListImage, Sort)
SELECT
  CarServiceId,
  CarServiceType,
  Name,
  CASE WHEN CarServiceType = 0 AND Sort >= 5 THEN Body ELSE NULL END,
  CASE WHEN CarServiceType = 3 THEN Body ELSE NULL END,
  CASE
    WHEN CarServiceType = 1 AND Sort = 1 THEN N'seed/gallery-1.webp'
    WHEN CarServiceType = 1 AND Sort = 2 THEN N'seed/gallery-2.webp'
    WHEN CarServiceType = 1 AND Sort = 3 THEN N'seed/gallery-3.webp'
    ELSE NULL
  END,
  NULL,
  Sort
FROM D;

-- Clone details pattern to other services (3-7)
INSERT INTO dbo.CarServiceDetails (CarServiceId, CarServiceType, Name, Desciption, Body, Image, ListImage, Sort)
SELECT s.Id, d.CarServiceType, d.Name, d.Desciption, d.Body, d.Image, d.ListImage, d.Sort
FROM dbo.CarServices s
CROSS JOIN dbo.CarServiceDetails d
WHERE s.Id IN (3,4,5,6,7) AND d.CarServiceId = 2;

-- Testimonial banner GroupId=5
IF NOT EXISTS (SELECT 1 FROM dbo.Banners WHERE GroupId = 5 AND BannerName = N'Nguyễn Minh Tuấn')
BEGIN
  INSERT INTO dbo.Banners (BannerName, Slogan, Image, Active, GroupId, Url, Sort, Content, ListImage)
  VALUES (
    N'Nguyễn Minh Tuấn',
    N'Xe sạch, tài xế đúng giờ và rất lịch sự. Đặt xe đi tỉnh rất yên tâm.',
    N'seed/avatar.webp',
    1, 5, NULL, 1,
    N'Xe sạch, tài xế đúng giờ và rất lịch sự. Đặt xe đi tỉnh rất yên tâm. Sẽ tiếp tục dùng dịch vụ Hà Linh.',
    NULL
  );
END
ELSE
BEGIN
  UPDATE dbo.Banners SET
    Active = 1,
    Slogan = N'Xe sạch, tài xế đúng giờ và rất lịch sự. Đặt xe đi tỉnh rất yên tâm.',
    Content = N'Xe sạch, tài xế đúng giờ và rất lịch sự. Đặt xe đi tỉnh rất yên tâm. Sẽ tiếp tục dùng dịch vụ Hà Linh.',
    Image = COALESCE(NULLIF(Image, ''), N'seed/avatar.webp')
  WHERE GroupId = 5 AND BannerName = N'Nguyễn Minh Tuấn';
END

SELECT 'prices' AS k, COUNT(*) AS c FROM dbo.CarServicePrices
UNION ALL SELECT 'details', COUNT(*) FROM dbo.CarServiceDetails
UNION ALL SELECT 'banners5', COUNT(*) FROM dbo.Banners WHERE GroupId = 5 AND Active = 1;
