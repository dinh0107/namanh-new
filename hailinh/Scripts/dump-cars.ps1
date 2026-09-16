param([string]$ConnectionString)
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$c = New-Object System.Data.SqlClient.SqlConnection $ConnectionString
$c.Open()
$cmd = $c.CreateCommand()
$cmd.CommandText = @"
SELECT 'Car' k, Id, Title, Slogan, Capacity, Firm, Type, Speed FROM CarServices
SELECT 'Banner' k, Id, BannerName, Slogan, Content FROM Banners WHERE Active=1 AND (Slogan LIKE '%' + NCHAR(0x00C3) + '%' OR Content LIKE '%' + NCHAR(0x00C3) + '%' OR Slogan LIKE '%' + NCHAR(0x00E2) + '%' OR Slogan LIKE N'%Chuy%')
"@
# Simpler: dump all car + home-related banners
$cmd.CommandText = 'SELECT Id, Title, Slogan, Capacity, Firm, Type, Speed FROM CarServices ORDER BY Id'
$r = $cmd.ExecuteReader()
Write-Output '=== CarServices ==='
while ($r.Read()) {
  Write-Output ([string]::Format('Id={0} Slogan=[{1}] Cap=[{2}] Firm=[{3}] Type=[{4}] Speed=[{5}]', $r['Id'], $r['Slogan'], $r['Capacity'], $r['Firm'], $r['Type'], $r['Speed']))
}
$r.Close()

$cmd2 = $c.CreateCommand()
$cmd2.CommandText = 'SELECT Id, GroupId, BannerName, LEFT(ISNULL(Slogan,''''),120) Slogan, LEFT(ISNULL(Content,''''),120) Content FROM Banners WHERE Active=1 ORDER BY GroupId, Sort'
$r2 = $cmd2.ExecuteReader()
Write-Output '=== Banners ==='
while ($r2.Read()) {
  Write-Output ([string]::Format('Id={0} G={1} Name={2} Slogan=[{3}]', $r2['Id'], $r2['GroupId'], $r2['BannerName'], $r2['Slogan']))
}
$c.Close()
