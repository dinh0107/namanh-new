param(
  [Parameter(Mandatory = $true)]
  [string]$ConnectionString
)

[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

function U([string]$s) { return [regex]::Unescape($s) }

# Longer patterns first so "45" is not matched as "4"
$rows = @(
  @{ Match = 'Limousine'; Capacity = 'Limousine VIP'; Speed = (U 'N\u1ed9i th\u1ea5t cao c\u1ea5p'); Type = (U 'VIP, ri\u00eang t\u01b0'); Firm = 'Limousine' },
  @{ Match = '45'; Capacity = (U '45 ch\u1ed7'); Speed = (U '\u0110i\u1ec1u h\u00f2a, gh\u1ebf ng\u1ed3i \u00eam'); Type = (U 'Tour l\u1edbn, h\u1ed9i ngh\u1ecb'); Firm = 'Universe' },
  @{ Match = '29'; Capacity = (U '29 ch\u1ed7'); Speed = (U '\u0110i\u1ec1u h\u00f2a, r\u1ed9ng r\u00e3i'); Type = (U '\u0110o\u00e0n th\u1ec3, s\u1ef1 ki\u1ec7n, du l\u1ecbch'); Firm = 'Hyundai / Samco' },
  @{ Match = '16'; Capacity = (U '16 ch\u1ed7'); Speed = (U '\u0110i\u1ec1u h\u00f2a, \u00e2m thanh'); Type = (U '\u0110o\u00e0n nh\u00f3m, s\u1ef1 ki\u1ec7n'); Firm = 'Ford / Hyundai' },
  @{ Match = '7'; Capacity = (U '7 ch\u1ed7'); Speed = (U '\u0110i\u1ec1u h\u00f2a 2 v\u00f9ng'); Type = (U 'Gia \u0111\u00ecnh, \u0111i t\u1ec9nh'); Firm = 'Toyota' },
  @{ Match = '4'; Capacity = (U '4-5 ch\u1ed7'); Speed = (U '\u0110i\u1ec1u h\u00f2a m\u00e1t l\u1ea1nh'); Type = (U 'S\u00e2n bay, c\u00f4ng t\u00e1c, \u0111i t\u1ec9nh'); Firm = 'Toyota / Hyundai' }
)

$sloganByMatch = @{
  '4' = (U 'Nh\u1ecf g\u1ecdn \u2013 Ti\u1ebft ki\u1ec7m \u2013 Linh ho\u1ea1t')
  '7' = (U 'R\u1ed9ng r\u00e3i \u2013 \u00cam \u00e1i \u2013 Gia \u0111\u00ecnh')
  '16' = (U '\u0110o\u00e0n nh\u00f3m \u2013 Du l\u1ecbch \u2013 H\u1ed9i ngh\u1ecb')
  '29' = (U 'Tho\u1ea3i m\u00e1i \u2013 Chuy\u00ean nghi\u1ec7p \u2013 \u0110\u00fang gi\u1edd')
  '45' = (U 'S\u1ee9c ch\u1ee9a l\u1edbn \u2013 An to\u00e0n \u2013 Ti\u1ec7n nghi')
  'Limousine' = (U 'Cao c\u1ea5p \u2013 Ri\u00eang t\u01b0 \u2013 Tho\u1ea3i m\u00e1i')
}

# Mojibake marker: UTF-8 C4 91 (d-stroke) misread as Latin-1, or E2 80 (en-dash) misread
function Looks-Broken([string]$text) {
  if ([string]::IsNullOrWhiteSpace($text)) { return $false }
  $hasC4 = $text.IndexOf([char]0x00C4) -ge 0
  $hasE1BB = $text.Contains(([char]0x00E1).ToString() + ([char]0x00BB).ToString())
  $hasE280 = $text.Contains(([char]0x00E2).ToString() + ([char]0x0080).ToString())
  # Also catch "Xe " + broken "doi moi" leftover from wrong seed
  $looksLikeBadge = $text -like '*doi*' -or $text -like '*Xe *' -and $hasC4
  return ($hasC4 -or $hasE1BB -or $hasE280)
}

$c = New-Object System.Data.SqlClient.SqlConnection $ConnectionString
$c.Open()

$cmd = $c.CreateCommand()
$cmd.CommandText = 'SELECT Id, Title, Slogan, Slug, Capacity, Firm, Type, Speed FROM CarServices'
$dt = New-Object System.Data.DataTable
$da = New-Object System.Data.SqlClient.SqlDataAdapter $cmd
[void]$da.Fill($dt)

foreach ($row in $dt.Rows) {
  $id = [int]$row['Id']
  $title = [string]$row['Title']
  $slug = [string]$row['Slug']
  $cap = [string]$row['Capacity']
  $firm = [string]$row['Firm']
  $type = [string]$row['Type']
  $speed = [string]$row['Speed']
  $slogan = [string]$row['Slogan']
  $hay = "$title $slug"

  $pick = $null
  $matchKey = $null
  foreach ($r in $rows) {
    if ($hay -like ("*" + $r.Match + "*")) { $pick = $r; $matchKey = $r.Match; break }
  }
  if ($null -eq $pick) { continue }

  $brokenFleet = (Looks-Broken $cap) -or (Looks-Broken $firm) -or (Looks-Broken $type) -or (Looks-Broken $speed) `
    -or ($cap -like '*x* chuy*') -or ($firm -like 'Xe *') -or ($type -like 'Chi *')

  if ($brokenFleet) {
    $upd = $c.CreateCommand()
    $upd.CommandText = 'UPDATE CarServices SET Capacity=@c, Speed=@s, Type=@t, Firm=@f WHERE Id=@id'
    [void]$upd.Parameters.AddWithValue('@c', $pick.Capacity)
    [void]$upd.Parameters.AddWithValue('@s', $pick.Speed)
    [void]$upd.Parameters.AddWithValue('@t', $pick.Type)
    [void]$upd.Parameters.AddWithValue('@f', $pick.Firm)
    [void]$upd.Parameters.AddWithValue('@id', $id)
    [void]$upd.ExecuteNonQuery()
    Write-Output ("Fixed fleet Id=$id")
  }

  if ((Looks-Broken $slogan) -and $sloganByMatch.ContainsKey($matchKey)) {
    $upd2 = $c.CreateCommand()
    $upd2.CommandText = 'UPDATE CarServices SET Slogan=@sg WHERE Id=@id'
    [void]$upd2.Parameters.AddWithValue('@sg', $sloganByMatch[$matchKey])
    [void]$upd2.Parameters.AddWithValue('@id', $id)
    [void]$upd2.ExecuteNonQuery()
    Write-Output ("Fixed slogan Id=$id")
  }
}

$check = $c.CreateCommand()
$check.CommandText = 'SELECT Id, Title, Capacity, Speed, Type, Firm, Slogan FROM CarServices ORDER BY Id'
$r = $check.ExecuteReader()
while ($r.Read()) {
  $line = [string]::Format('{0} | {1} | Cap={2} | Speed={3} | Type={4} | Firm={5} | Slogan={6}', $r['Id'], $r['Title'], $r['Capacity'], $r['Speed'], $r['Type'], $r['Firm'], $r['Slogan'])
  Write-Output $line
}
$c.Close()
