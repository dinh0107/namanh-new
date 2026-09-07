-- Cháº¡y trÃªn database hailinh TRÆ¯á»šC khi Update-Database náº¿u migration bá»‹ káº¹t.
-- SQL Server Management Studio hoáº·c Visual Studio > SQL Server Object Explorer

-- 1) XÃ³a record migration báº£ng giÃ¡ (Ä‘á»ƒ EF cháº¡y láº¡i tá»« Ä‘áº§u)
DELETE FROM dbo.__MigrationHistory
WHERE MigrationId IN (
    N'202606141500000_add-location-price-fields',
    N'202606141600000_seed-price-routes-mock',
    N'202606141700000_sync-price-routes-model',
    N'202606141800000_add-location-single-price'
);

-- 2) Kiá»ƒm tra cÃ¡c migration Ä‘Ã£ apply
SELECT MigrationId, ContextKey FROM dbo.__MigrationHistory ORDER BY MigrationId;
