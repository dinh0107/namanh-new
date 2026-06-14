-- Thêm cột Price linh hoạt (mỗi tuyến 1 giá theo tab loại xe)
-- Chạy nếu chưa Update-Database được migration 202606141800000_add-location-single-price

IF COL_LENGTH('dbo.Locations', 'Price') IS NULL
BEGIN
    ALTER TABLE dbo.Locations ADD Price NVARCHAR(MAX) NULL;
END

UPDATE dbo.Locations
SET Price = COALESCE(NULLIF(LTRIM(RTRIM(Price)), ''), Price4, Price7, Price16, Price29, Price45, PriceLim)
WHERE COALESCE(NULLIF(LTRIM(RTRIM(Price)), ''), Price4, Price7, Price16, Price29, Price45, PriceLim) IS NOT NULL;
