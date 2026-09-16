param(
  [string]$ConnectionString
)

$c = New-Object System.Data.SqlClient.SqlConnection $ConnectionString
$c.Open()
$cmd = $c.CreateCommand()
$cmd.CommandText = "SELECT Id, Title, Slogan, Firm, Capacity, Type, Speed FROM CarServices ORDER BY Id"
$r = $cmd.ExecuteReader()
while ($r.Read()) {
  $line = "{0} | {1} | Slogan={2} | Firm={3} | Cap={4} | Type={5} | Speed={6}" -f `
    $r["Id"], $r["Title"], $r["Slogan"], $r["Firm"], $r["Capacity"], $r["Type"], $r["Speed"]
  [Console]::OutputEncoding = [System.Text.Encoding]::UTF8
  Write-Output $line
}
$c.Close()
