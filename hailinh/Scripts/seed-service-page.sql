-- Seed ảnh + testimonial trang dịch vụ (không ghi đè Firm/Capacity/Type/Speed — dùng cho trang chủ)
UPDATE dbo.CarServices SET
    ImageUrl = COALESCE(NULLIF(LTRIM(RTRIM(ImageUrl)), N''), N'seed/hero-vios.webp'),
    Image = COALESCE(NULLIF(LTRIM(RTRIM(Image)), N''), N'seed/hero-vios.webp')
WHERE ImageUrl IS NULL OR LTRIM(RTRIM(ImageUrl)) = N'';

UPDATE d SET d.Image = N'seed/gallery-1.webp'
FROM dbo.CarServiceDetails d WHERE d.CarServiceType = 1 AND d.Sort = 1 AND (d.Image IS NULL OR d.Image = N'');

UPDATE d SET d.Image = N'seed/gallery-2.webp'
FROM dbo.CarServiceDetails d WHERE d.CarServiceType = 1 AND d.Sort = 2 AND (d.Image IS NULL OR d.Image = N'');

UPDATE d SET d.Image = N'seed/gallery-3.webp'
FROM dbo.CarServiceDetails d WHERE d.CarServiceType = 1 AND d.Sort = 3 AND (d.Image IS NULL OR d.Image = N'');

IF NOT EXISTS (SELECT 1 FROM dbo.Banners WHERE GroupId = 5 AND Active = 1 AND Image IS NOT NULL)
    INSERT INTO dbo.Banners (BannerName, Slogan, Image, Active, GroupId, Sort, Content)
    VALUES (N'Nguyễn Minh Tuấn', N'Khách hàng thân thiết', N'seed/avatar.webp', 1, 5, 99,
        N'<p>Dịch vụ rất chuyên nghiệp, tài xế đúng giờ và xe sạch sẽ.</p>');
