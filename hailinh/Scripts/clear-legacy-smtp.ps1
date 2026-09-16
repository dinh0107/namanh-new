param([string]$ConnectionString)
$c = New-Object System.Data.SqlClient.SqlConnection $ConnectionString
$c.Open()
$cmd = $c.CreateCommand()
$cmd.CommandText = @"
IF COL_LENGTH(N'dbo.ConfigSites', N'SmtpEmail') IS NOT NULL
BEGIN
  UPDATE dbo.ConfigSites SET SmtpEmail=NULL, SmtpPassword=NULL WHERE SmtpEmail=N'kythuatluatankhang@gmail.com';
END
IF COL_LENGTH(N'dbo.ConfigSites', N'Email') IS NOT NULL
BEGIN
  UPDATE dbo.ConfigSites SET Email=NULL WHERE Email=N'kythuatluatankhang@gmail.com';
END
SELECT TOP 1 ISNULL(SmtpEmail,'(null)') AS SmtpEmail, ISNULL(Email,'(null)') AS Email FROM dbo.ConfigSites
"@
$r = $cmd.ExecuteReader()
while ($r.Read()) {
  Write-Output ("SmtpEmail={0} | Email={1}" -f $r['SmtpEmail'], $r['Email'])
}
$c.Close()
