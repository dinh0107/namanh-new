-- Fix homepage fleet card fields (Firm/Capacity/Type/Speed) corrupted by bad seed encoding.
-- Run with: sqlcmd ... -f 65001 -i fix-fleet-card-fields.sql
SET NOCOUNT ON;

UPDATE dbo.CarServices SET
  Capacity = N'4-5 chỗ',
  Speed = N'Điều hòa mát lạnh',
  Type = N'Sân bay, công tác, đi tỉnh',
  Firm = N'Toyota / Hyundai'
WHERE Id = 2 OR Title LIKE N'%4%5%' OR Slug LIKE N'%4-cho%';

UPDATE dbo.CarServices SET
  Capacity = N'7 chỗ',
  Speed = N'Điều hòa 2 vùng',
  Type = N'Gia đình, đi tỉnh',
  Firm = N'Toyota'
WHERE Id = 3 OR Title LIKE N'%7 chỗ%' OR Slug LIKE N'%7-cho%';

UPDATE dbo.CarServices SET
  Capacity = N'16 chỗ',
  Speed = N'Điều hòa, âm thanh',
  Type = N'Đoàn nhóm, sự kiện',
  Firm = N'Ford / Hyundai'
WHERE Id = 4 OR Title LIKE N'%16%' OR Slug LIKE N'%16-cho%';

UPDATE dbo.CarServices SET
  Capacity = N'29 chỗ',
  Speed = N'Điều hòa, rộng rãi',
  Type = N'Đoàn thể, sự kiện, du lịch',
  Firm = N'Hyundai / Samco'
WHERE Id = 5 OR Title LIKE N'%29%' OR Slug LIKE N'%29-cho%';

UPDATE dbo.CarServices SET
  Capacity = N'45 chỗ',
  Speed = N'Điều hòa, ghế ngồi êm',
  Type = N'Tour lớn, hội nghị',
  Firm = N'Universe'
WHERE Id = 6 OR Title LIKE N'%45%' OR Slug LIKE N'%45-cho%';

UPDATE dbo.CarServices SET
  Capacity = N'Limousine VIP',
  Speed = N'Nội thất cao cấp',
  Type = N'VIP, riêng tư',
  Firm = N'Limousine'
WHERE Id = 7 OR Title LIKE N'%Limousine%' OR Slug LIKE N'%limousine%';

-- Fallback: any still-garbled Capacity (mojibake markers)
UPDATE dbo.CarServices SET
  Capacity = COALESCE(NULLIF(LTRIM(RTRIM(Car)), N''), Title),
  Speed = N'Điều hòa mát lạnh',
  Type = N'Du lịch, công tác',
  Firm = N'Hà Linh'
WHERE Capacity LIKE N'%Ã%' OR Capacity LIKE N'%Ä%' OR Capacity LIKE N'%á»%'
   OR Firm LIKE N'%Ã%' OR Firm LIKE N'%Ä%' OR Firm LIKE N'%á»%';

SELECT Id, Title, Capacity, Speed, Type, Firm, Slogan FROM dbo.CarServices ORDER BY Id;
