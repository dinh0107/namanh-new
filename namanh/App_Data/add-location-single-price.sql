-- Fix lỗi "Invalid column name 'Price'" khi Update-Database
-- Chạy file này trên database namanh (SSMS / SQL Server Object Explorer)

-- Bước 1: Thêm cột Price nếu chưa có
IF COL_LENGTH('dbo.Locations', 'Price') IS NULL
BEGIN
    ALTER TABLE dbo.Locations ADD Price NVARCHAR(MAX) NULL;
    PRINT 'Da them cot Price';
END
ELSE
BEGIN
    PRINT 'Cot Price da ton tai';
END

-- Bước 2: Copy du lieu cu (neu co cot Price4...)
IF COL_LENGTH('dbo.Locations', 'Price4') IS NOT NULL
BEGIN
    UPDATE dbo.Locations
    SET Price = COALESCE(NULLIF(LTRIM(RTRIM(Price)), ''), Price4, Price7, Price16, Price29, Price45, PriceLim)
    WHERE COALESCE(NULLIF(LTRIM(RTRIM(Price)), ''), Price4, Price7, Price16, Price29, Price45, PriceLim) IS NOT NULL;
    PRINT 'Da copy gia cu sang Price';
END

-- Bước 3: Neu migration 180000 bi loi nhung da ghi vao history, xoa de chay lai Update-Database
-- (Chi chay neu Update-Database bao "No pending migrations" nhung cot Price van thieu)
/*
DELETE FROM dbo.__MigrationHistory
WHERE MigrationId = N'202606141800000_add-location-single-price';
*/

-- Bước 4: Kiem tra
SELECT TOP 5 Id, Name, Price, Price4, Price7 FROM dbo.Locations;
SELECT MigrationId FROM dbo.__MigrationHistory ORDER BY MigrationId;
