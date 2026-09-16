param([Parameter(Mandatory=$true)][string]$ConnectionString)
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
function U([string]$s) { return [regex]::Unescape($s) }

$name = U 'Nguy\u1ec5n Minh Tu\u1ea5n'
$slogan = U 'Kh\u00e1ch h\u00e0ng th\u00e2n thi\u1ebft'
$content = U 'Xe s\u1ea1ch, t\u00e0i x\u1ebf \u0111\u00fang gi\u1edd v\u00e0 r\u1ea5t l\u1ecbch s\u1ef1. \u0110\u1eb7t xe \u0111i t\u1ec9nh r\u1ea5t y\u00ean t\u00e2m. S\u1ebd ti\u1ebfp t\u1ee5c d\u00f9ng d\u1ecbch v\u1ee5 H\u00e0 Linh.'

function Looks-Broken([string]$text) {
  if ([string]::IsNullOrWhiteSpace($text)) { return $false }
  return ($text.IndexOf([char]0x00C3) -ge 0) -or
         ($text.IndexOf([char]0x00C4) -ge 0) -or
         ($text.Contains(([char]0x00E1).ToString() + ([char]0x00BB).ToString()))
}

$c = New-Object System.Data.SqlClient.SqlConnection $ConnectionString
$c.Open()
$cmd = $c.CreateCommand()
$cmd.CommandText = 'SELECT Id, BannerName, Slogan, Content FROM Banners WHERE GroupId = 5'
$dt = New-Object System.Data.DataTable
$da = New-Object System.Data.SqlClient.SqlDataAdapter $cmd
[void]$da.Fill($dt)

foreach ($row in $dt.Rows) {
  $bn = [string]$row['BannerName']
  $sg = [string]$row['Slogan']
  $ct = [string]$row['Content']
  if (-not ((Looks-Broken $bn) -or (Looks-Broken $sg) -or (Looks-Broken $ct))) { continue }
  $upd = $c.CreateCommand()
  $upd.CommandText = 'UPDATE Banners SET BannerName=@n, Slogan=@s, Content=@c WHERE Id=@id'
  [void]$upd.Parameters.AddWithValue('@n', $name)
  [void]$upd.Parameters.AddWithValue('@s', $slogan)
  [void]$upd.Parameters.AddWithValue('@c', $content)
  [void]$upd.Parameters.AddWithValue('@id', [int]$row['Id'])
  [void]$upd.ExecuteNonQuery()
  Write-Output ("Fixed banner Id=$($row['Id'])")
}
$c.Close()
