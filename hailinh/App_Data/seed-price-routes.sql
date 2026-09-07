-- Cháº¡y thá»§ cÃ´ng náº¿u Update-Database váº«n lá»—i.
-- Sau khi cháº¡y xong, cháº¡y reset-price-migrations.sql rá»“i Update-Database
-- (hoáº·c INSERT migration history â€” xem cuá»‘i file).

-- === SCHEMA ===
IF COL_LENGTH('dbo.Locations', 'Price4') IS NULL
BEGIN
    ALTER TABLE dbo.Locations ADD Price4 NVARCHAR(MAX) NULL;
    ALTER TABLE dbo.Locations ADD Price7 NVARCHAR(MAX) NULL;
    ALTER TABLE dbo.Locations ADD Price16 NVARCHAR(MAX) NULL;
    ALTER TABLE dbo.Locations ADD Price29 NVARCHAR(MAX) NULL;
    ALTER TABLE dbo.Locations ADD Price45 NVARCHAR(MAX) NULL;
    ALTER TABLE dbo.Locations ADD PriceLim NVARCHAR(MAX) NULL;
    ALTER TABLE dbo.Locations ADD Sort INT NOT NULL CONSTRAINT DF_Locations_Sort DEFAULT 1;
    ALTER TABLE dbo.Locations ADD Hot BIT NOT NULL CONSTRAINT DF_Locations_Hot DEFAULT 0;
    ALTER TABLE dbo.Locations ADD Active BIT NOT NULL CONSTRAINT DF_Locations_Active DEFAULT 1;
END
GO

-- === SEED ===
DELETE FROM dbo.Locations;
DELETE FROM dbo.PriceLangdings;
GO

SET IDENTITY_INSERT dbo.PriceLangdings ON;
INSERT INTO dbo.PriceLangdings (Id, Name, Description, Sort, Active) VALUES
(1, N'4 chá»—',     N'Báº£ng giÃ¡ thuÃª xe 4 chá»—', 1, 1),
(2, N'7 chá»—',     N'Báº£ng giÃ¡ thuÃª xe 7 chá»—', 2, 1),
(3, N'16 chá»—',    N'Báº£ng giÃ¡ thuÃª xe 16 chá»—', 3, 1),
(4, N'29 chá»—',    N'Báº£ng giÃ¡ thuÃª xe 29 chá»—', 4, 1),
(5, N'45 chá»—',    N'Báº£ng giÃ¡ thuÃª xe 45 chá»—', 5, 1),
(6, N'Limousine', N'Báº£ng giÃ¡ thuÃª xe Limousine', 6, 1);
SET IDENTITY_INSERT dbo.PriceLangdings OFF;
GO

SET IDENTITY_INSERT dbo.Locations ON;
INSERT INTO dbo.Locations (Id, Name, PriceLangdingId, Price4, Price7, Price16, Price29, Price45, PriceLim, Sort, Hot, Active) VALUES
(1,  N'LÃ o Cai',      1, N'3.000.000 Ä‘', NULL, NULL, NULL, NULL, NULL, 1, 1, 1),
(2,  N'Sapa',         1, N'3.200.000 Ä‘', NULL, NULL, NULL, NULL, NULL, 2, 0, 1),
(3,  N'HÃ  Giang',     1, N'3.000.000 Ä‘', NULL, NULL, NULL, NULL, NULL, 3, 0, 1),
(4,  N'Háº¡ Long',      1, N'2.000.000 Ä‘', NULL, NULL, NULL, NULL, NULL, 4, 1, 1),
(5,  N'Háº£i PhÃ²ng',    1, N'1.200.000 Ä‘', NULL, NULL, NULL, NULL, NULL, 5, 0, 1),
(6,  N'Nam Äá»‹nh',     1, N'800.000 Ä‘',   NULL, NULL, NULL, NULL, NULL, 6, 0, 1),
(7,  N'Thanh HÃ³a',   1, N'2.000.000 Ä‘', NULL, NULL, NULL, NULL, NULL, 7, 0, 1),
(8,  N'Nghá»‡ An',      1, N'3.000.000 Ä‘', NULL, NULL, NULL, NULL, NULL, 8, 0, 1),
(9,  N'SÆ¡n La',       1, N'3.000.000 Ä‘', NULL, NULL, NULL, NULL, NULL, 9, 0, 1),
(10, N'ThÃ¡i NguyÃªn',  1, N'1.000.000 Ä‘', NULL, NULL, NULL, NULL, NULL, 10, 0, 1),
(11, N'LÃ o Cai',      2, NULL, N'3.600.000 Ä‘', NULL, NULL, NULL, NULL, 1, 1, 1),
(12, N'Sapa',         2, NULL, N'3.800.000 Ä‘', NULL, NULL, NULL, NULL, 2, 0, 1),
(13, N'HÃ  Giang',     2, NULL, N'3.500.000 Ä‘', NULL, NULL, NULL, NULL, 3, 0, 1),
(14, N'Háº¡ Long',      2, NULL, N'2.400.000 Ä‘', NULL, NULL, NULL, NULL, 4, 1, 1),
(15, N'Háº£i PhÃ²ng',    2, NULL, N'1.500.000 Ä‘', NULL, NULL, NULL, NULL, 5, 0, 1),
(16, N'Nam Äá»‹nh',     2, NULL, N'1.000.000 Ä‘', NULL, NULL, NULL, NULL, 6, 0, 1),
(17, N'Thanh HÃ³a',   2, NULL, N'2.300.000 Ä‘', NULL, NULL, NULL, NULL, 7, 0, 1),
(18, N'Nghá»‡ An',      2, NULL, N'3.500.000 Ä‘', NULL, NULL, NULL, NULL, 8, 0, 1),
(19, N'SÆ¡n La',       2, NULL, N'3.500.000 Ä‘', NULL, NULL, NULL, NULL, 9, 0, 1),
(20, N'ThÃ¡i NguyÃªn',  2, NULL, N'1.500.000 Ä‘', NULL, NULL, NULL, NULL, 10, 0, 1),
(21, N'LÃ o Cai',      3, NULL, NULL, N'4.800.000 Ä‘', NULL, NULL, NULL, 1, 1, 1),
(22, N'Sapa',         3, NULL, NULL, N'5.200.000 Ä‘', NULL, NULL, NULL, 2, 0, 1),
(23, N'HÃ  Giang',     3, NULL, NULL, N'5.000.000 Ä‘', NULL, NULL, NULL, 3, 0, 1),
(24, N'Háº¡ Long',      3, NULL, NULL, N'2.800.000 Ä‘', NULL, NULL, NULL, 4, 1, 1),
(25, N'Háº£i PhÃ²ng',    3, NULL, NULL, N'2.200.000 Ä‘', NULL, NULL, NULL, 5, 0, 1),
(26, N'Nam Äá»‹nh',     3, NULL, NULL, N'1.400.000 Ä‘', NULL, NULL, NULL, 6, 0, 1),
(27, N'Thanh HÃ³a',   3, NULL, NULL, N'2.700.000 Ä‘', NULL, NULL, NULL, 7, 0, 1),
(28, N'Nghá»‡ An',      3, NULL, NULL, N'5.000.000 Ä‘', NULL, NULL, NULL, 8, 0, 1),
(29, N'SÆ¡n La',       3, NULL, NULL, N'5.000.000 Ä‘', NULL, NULL, NULL, 9, 0, 1),
(30, N'ThÃ¡i NguyÃªn',  3, NULL, NULL, N'1.800.000 Ä‘', NULL, NULL, NULL, 10, 0, 1),
(31, N'LÃ o Cai',      4, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, NULL, 1, 0, 1),
(32, N'Sapa',         4, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, NULL, 2, 0, 1),
(33, N'HÃ  Giang',     4, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, NULL, 3, 0, 1),
(34, N'Háº¡ Long',      4, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, NULL, 4, 0, 1),
(35, N'Háº£i PhÃ²ng',    4, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, NULL, 5, 0, 1),
(36, N'Nam Äá»‹nh',     4, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, NULL, 6, 0, 1),
(37, N'Thanh HÃ³a',   4, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, NULL, 7, 0, 1),
(38, N'Nghá»‡ An',      4, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, NULL, 8, 0, 1),
(39, N'SÆ¡n La',       4, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, NULL, 9, 0, 1),
(40, N'ThÃ¡i NguyÃªn',  4, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, NULL, 10, 0, 1),
(41, N'LÃ o Cai',      5, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, 1, 0, 1),
(42, N'Sapa',         5, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, 2, 0, 1),
(43, N'HÃ  Giang',     5, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, 3, 0, 1),
(44, N'Háº¡ Long',      5, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, 4, 0, 1),
(45, N'Háº£i PhÃ²ng',    5, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, 5, 0, 1),
(46, N'Nam Äá»‹nh',     5, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, 6, 0, 1),
(47, N'Thanh HÃ³a',   5, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, 7, 0, 1),
(48, N'Nghá»‡ An',      5, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, 8, 0, 1),
(49, N'SÆ¡n La',       5, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, 9, 0, 1),
(50, N'ThÃ¡i NguyÃªn',  5, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', NULL, 10, 0, 1),
(51, N'LÃ o Cai',      6, NULL, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', 1, 0, 1),
(52, N'Sapa',         6, NULL, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', 2, 0, 1),
(53, N'HÃ  Giang',     6, NULL, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', 3, 0, 1),
(54, N'Háº¡ Long',      6, NULL, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', 4, 0, 1),
(55, N'Háº£i PhÃ²ng',    6, NULL, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', 5, 0, 1),
(56, N'Nam Äá»‹nh',     6, NULL, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', 6, 0, 1),
(57, N'Thanh HÃ³a',   6, NULL, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', 7, 0, 1),
(58, N'Nghá»‡ An',      6, NULL, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', 8, 0, 1),
(59, N'SÆ¡n La',       6, NULL, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', 9, 0, 1),
(60, N'ThÃ¡i NguyÃªn',  6, NULL, NULL, NULL, NULL, NULL, N'LiÃªn há»‡', 10, 0, 1);
SET IDENTITY_INSERT dbo.Locations OFF;
GO

-- === ÄÃ¡nh dáº¥u migration Ä‘Ã£ cháº¡y (sau khi schema + seed OK) ===
-- Chá»‰ cháº¡y náº¿u KHÃ”NG dÃ¹ng Update-Database ná»¯a:
/*
INSERT INTO dbo.__MigrationHistory (MigrationId, ContextKey, Model, ProductVersion)
SELECT N'202606141500000_add-location-price-fields', N'hailinh.Migrations.Configuration', Model, ProductVersion
FROM dbo.__MigrationHistory WHERE MigrationId = N'202605100352323_add-table-price';

INSERT INTO dbo.__MigrationHistory (MigrationId, ContextKey, Model, ProductVersion)
SELECT N'202606141600000_seed-price-routes-mock', N'hailinh.Migrations.Configuration', Model, ProductVersion
FROM dbo.__MigrationHistory WHERE MigrationId = N'202606141500000_add-location-price-fields';

INSERT INTO dbo.__MigrationHistory (MigrationId, ContextKey, Model, ProductVersion)
SELECT N'202606141700000_sync-price-routes-model', N'hailinh.Migrations.Configuration', Model, ProductVersion
FROM dbo.__MigrationHistory WHERE MigrationId = N'202606141600000_seed-price-routes-mock';
*/
