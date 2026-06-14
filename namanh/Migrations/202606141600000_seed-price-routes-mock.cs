namespace namanh.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class seedpriceroutesmock : DbMigration
    {
        public override void Up()
        {
            Sql(@"
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

DELETE FROM dbo.Locations;
DELETE FROM dbo.PriceLangdings;

SET IDENTITY_INSERT dbo.PriceLangdings ON;
INSERT INTO dbo.PriceLangdings (Id, Name, Description, Sort, Active) VALUES
(1, N'4 chỗ',     N'Bảng giá thuê xe 4 chỗ', 1, 1),
(2, N'7 chỗ',     N'Bảng giá thuê xe 7 chỗ', 2, 1),
(3, N'16 chỗ',    N'Bảng giá thuê xe 16 chỗ', 3, 1),
(4, N'29 chỗ',    N'Bảng giá thuê xe 29 chỗ', 4, 1),
(5, N'45 chỗ',    N'Bảng giá thuê xe 45 chỗ', 5, 1),
(6, N'Limousine', N'Bảng giá thuê xe Limousine', 6, 1);
SET IDENTITY_INSERT dbo.PriceLangdings OFF;

SET IDENTITY_INSERT dbo.Locations ON;

-- Tab 4 chỗ
INSERT INTO dbo.Locations (Id, Name, PriceLangdingId, Price4, Price7, Price16, Price29, Price45, PriceLim, Sort, Hot, Active) VALUES
(1,  N'Lào Cai',      1, N'3.000.000 đ', NULL, NULL, NULL, NULL, NULL, 1, 1, 1),
(2,  N'Sapa',         1, N'3.200.000 đ', NULL, NULL, NULL, NULL, NULL, 2, 0, 1),
(3,  N'Hà Giang',     1, N'3.000.000 đ', NULL, NULL, NULL, NULL, NULL, 3, 0, 1),
(4,  N'Hạ Long',      1, N'2.000.000 đ', NULL, NULL, NULL, NULL, NULL, 4, 1, 1),
(5,  N'Hải Phòng',    1, N'1.200.000 đ', NULL, NULL, NULL, NULL, NULL, 5, 0, 1),
(6,  N'Nam Định',     1, N'800.000 đ',   NULL, NULL, NULL, NULL, NULL, 6, 0, 1),
(7,  N'Thanh Hóa',   1, N'2.000.000 đ', NULL, NULL, NULL, NULL, NULL, 7, 0, 1),
(8,  N'Nghệ An',      1, N'3.000.000 đ', NULL, NULL, NULL, NULL, NULL, 8, 0, 1),
(9,  N'Sơn La',       1, N'3.000.000 đ', NULL, NULL, NULL, NULL, NULL, 9, 0, 1),
(10, N'Thái Nguyên',  1, N'1.000.000 đ', NULL, NULL, NULL, NULL, NULL, 10, 0, 1),

-- Tab 7 chỗ
(11, N'Lào Cai',      2, NULL, N'3.600.000 đ', NULL, NULL, NULL, NULL, 1, 1, 1),
(12, N'Sapa',         2, NULL, N'3.800.000 đ', NULL, NULL, NULL, NULL, 2, 0, 1),
(13, N'Hà Giang',     2, NULL, N'3.500.000 đ', NULL, NULL, NULL, NULL, 3, 0, 1),
(14, N'Hạ Long',      2, NULL, N'2.400.000 đ', NULL, NULL, NULL, NULL, 4, 1, 1),
(15, N'Hải Phòng',    2, NULL, N'1.500.000 đ', NULL, NULL, NULL, NULL, 5, 0, 1),
(16, N'Nam Định',     2, NULL, N'1.000.000 đ', NULL, NULL, NULL, NULL, 6, 0, 1),
(17, N'Thanh Hóa',   2, NULL, N'2.300.000 đ', NULL, NULL, NULL, NULL, 7, 0, 1),
(18, N'Nghệ An',      2, NULL, N'3.500.000 đ', NULL, NULL, NULL, NULL, 8, 0, 1),
(19, N'Sơn La',       2, NULL, N'3.500.000 đ', NULL, NULL, NULL, NULL, 9, 0, 1),
(20, N'Thái Nguyên',  2, NULL, N'1.500.000 đ', NULL, NULL, NULL, NULL, 10, 0, 1),

-- Tab 16 chỗ
(21, N'Lào Cai',      3, NULL, NULL, N'4.800.000 đ', NULL, NULL, NULL, 1, 1, 1),
(22, N'Sapa',         3, NULL, NULL, N'5.200.000 đ', NULL, NULL, NULL, 2, 0, 1),
(23, N'Hà Giang',     3, NULL, NULL, N'5.000.000 đ', NULL, NULL, NULL, 3, 0, 1),
(24, N'Hạ Long',      3, NULL, NULL, N'2.800.000 đ', NULL, NULL, NULL, 4, 1, 1),
(25, N'Hải Phòng',    3, NULL, NULL, N'2.200.000 đ', NULL, NULL, NULL, 5, 0, 1),
(26, N'Nam Định',     3, NULL, NULL, N'1.400.000 đ', NULL, NULL, NULL, 6, 0, 1),
(27, N'Thanh Hóa',   3, NULL, NULL, N'2.700.000 đ', NULL, NULL, NULL, 7, 0, 1),
(28, N'Nghệ An',      3, NULL, NULL, N'5.000.000 đ', NULL, NULL, NULL, 8, 0, 1),
(29, N'Sơn La',       3, NULL, NULL, N'5.000.000 đ', NULL, NULL, NULL, 9, 0, 1),
(30, N'Thái Nguyên',  3, NULL, NULL, N'1.800.000 đ', NULL, NULL, NULL, 10, 0, 1),

-- Tab 29 chỗ
(31, N'Lào Cai',      4, NULL, NULL, NULL, N'Liên hệ', NULL, NULL, 1, 0, 1),
(32, N'Sapa',         4, NULL, NULL, NULL, N'Liên hệ', NULL, NULL, 2, 0, 1),
(33, N'Hà Giang',     4, NULL, NULL, NULL, N'Liên hệ', NULL, NULL, 3, 0, 1),
(34, N'Hạ Long',      4, NULL, NULL, NULL, N'Liên hệ', NULL, NULL, 4, 0, 1),
(35, N'Hải Phòng',    4, NULL, NULL, NULL, N'Liên hệ', NULL, NULL, 5, 0, 1),
(36, N'Nam Định',     4, NULL, NULL, NULL, N'Liên hệ', NULL, NULL, 6, 0, 1),
(37, N'Thanh Hóa',   4, NULL, NULL, NULL, N'Liên hệ', NULL, NULL, 7, 0, 1),
(38, N'Nghệ An',      4, NULL, NULL, NULL, N'Liên hệ', NULL, NULL, 8, 0, 1),
(39, N'Sơn La',       4, NULL, NULL, NULL, N'Liên hệ', NULL, NULL, 9, 0, 1),
(40, N'Thái Nguyên',  4, NULL, NULL, NULL, N'Liên hệ', NULL, NULL, 10, 0, 1),

-- Tab 45 chỗ
(41, N'Lào Cai',      5, NULL, NULL, NULL, NULL, N'Liên hệ', NULL, 1, 0, 1),
(42, N'Sapa',         5, NULL, NULL, NULL, NULL, N'Liên hệ', NULL, 2, 0, 1),
(43, N'Hà Giang',     5, NULL, NULL, NULL, NULL, N'Liên hệ', NULL, 3, 0, 1),
(44, N'Hạ Long',      5, NULL, NULL, NULL, NULL, N'Liên hệ', NULL, 4, 0, 1),
(45, N'Hải Phòng',    5, NULL, NULL, NULL, NULL, N'Liên hệ', NULL, 5, 0, 1),
(46, N'Nam Định',     5, NULL, NULL, NULL, NULL, N'Liên hệ', NULL, 6, 0, 1),
(47, N'Thanh Hóa',   5, NULL, NULL, NULL, NULL, N'Liên hệ', NULL, 7, 0, 1),
(48, N'Nghệ An',      5, NULL, NULL, NULL, NULL, N'Liên hệ', NULL, 8, 0, 1),
(49, N'Sơn La',       5, NULL, NULL, NULL, NULL, N'Liên hệ', NULL, 9, 0, 1),
(50, N'Thái Nguyên',  5, NULL, NULL, NULL, NULL, N'Liên hệ', NULL, 10, 0, 1),

-- Tab Limousine
(51, N'Lào Cai',      6, NULL, NULL, NULL, NULL, NULL, N'Liên hệ', 1, 0, 1),
(52, N'Sapa',         6, NULL, NULL, NULL, NULL, NULL, N'Liên hệ', 2, 0, 1),
(53, N'Hà Giang',     6, NULL, NULL, NULL, NULL, NULL, N'Liên hệ', 3, 0, 1),
(54, N'Hạ Long',      6, NULL, NULL, NULL, NULL, NULL, N'Liên hệ', 4, 0, 1),
(55, N'Hải Phòng',    6, NULL, NULL, NULL, NULL, NULL, N'Liên hệ', 5, 0, 1),
(56, N'Nam Định',     6, NULL, NULL, NULL, NULL, NULL, N'Liên hệ', 6, 0, 1),
(57, N'Thanh Hóa',   6, NULL, NULL, NULL, NULL, NULL, N'Liên hệ', 7, 0, 1),
(58, N'Nghệ An',      6, NULL, NULL, NULL, NULL, NULL, N'Liên hệ', 8, 0, 1),
(59, N'Sơn La',       6, NULL, NULL, NULL, NULL, NULL, N'Liên hệ', 9, 0, 1),
(60, N'Thái Nguyên',  6, NULL, NULL, NULL, NULL, NULL, N'Liên hệ', 10, 0, 1);

SET IDENTITY_INSERT dbo.Locations OFF;
");
        }

        public override void Down()
        {
            Sql(@"
DELETE FROM dbo.Locations;
DELETE FROM dbo.PriceLangdings;
");
        }
    }
}
