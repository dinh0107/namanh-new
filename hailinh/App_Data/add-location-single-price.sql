-- Fix loi Invalid column name 'Price' khi Update-Database
-- Chay tren database hailinh (SSMS), sau do chay Update-Database trong PMC

-- Buoc 1: Xoa migration loi de chay lai
DELETE FROM dbo.__MigrationHistory
WHERE MigrationId = N'202606141800000_add-location-single-price';

-- Buoc 2: Them cot Price (tach rieng khoi UPDATE)
IF COL_LENGTH('dbo.Locations', 'Price') IS NULL
BEGIN
    ALTER TABLE dbo.Locations ADD Price NVARCHAR(MAX) NULL;
END

-- Buoc 3: Copy gia cu sang cot Price
IF COL_LENGTH('dbo.Locations', 'Price4') IS NOT NULL
BEGIN
    UPDATE dbo.Locations
    SET Price = COALESCE(NULLIF(LTRIM(RTRIM(Price)), ''), Price4, Price7, Price16, Price29, Price45, PriceLim)
    WHERE COALESCE(NULLIF(LTRIM(RTRIM(Price)), ''), Price4, Price7, Price16, Price29, Price45, PriceLim) IS NOT NULL;
END

-- Buoc 4: Kiem tra
SELECT TOP 5 Id, Name, Price FROM dbo.Locations;

-- Buoc 5: Trong Package Manager Console chay:
-- Update-Database
