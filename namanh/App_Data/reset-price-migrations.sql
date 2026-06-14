-- Chạy trên database namanh TRƯỚC khi Update-Database nếu migration bị kẹt.
-- SQL Server Management Studio hoặc Visual Studio > SQL Server Object Explorer

-- 1) Xóa record migration bảng giá (để EF chạy lại từ đầu)
DELETE FROM dbo.__MigrationHistory
WHERE MigrationId IN (
    N'202606141500000_add-location-price-fields',
    N'202606141600000_seed-price-routes-mock',
    N'202606141700000_sync-price-routes-model',
    N'202606141800000_add-location-single-price'
);

-- 2) Kiểm tra các migration đã apply
SELECT MigrationId, ContextKey FROM dbo.__MigrationHistory ORDER BY MigrationId;
