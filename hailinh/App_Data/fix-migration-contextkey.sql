-- Chạy 1 lần trên DB production nếu vẫn lỗi "already an object named Admins"
-- (sau khi đổi namespace hailinh, ContextKey trong __MigrationHistory phải khớp)

UPDATE dbo.__MigrationHistory
SET ContextKey = N'hailinh.Migrations.Configuration'
WHERE ContextKey <> N'hailinh.Migrations.Configuration';

SELECT ContextKey, COUNT(*) AS Cnt
FROM dbo.__MigrationHistory
GROUP BY ContextKey;
