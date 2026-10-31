# PowerShell 7. Run locally; no secret values are printed or written to this repository.
[CmdletBinding()]
param()
$ErrorActionPreference = "Stop"
$project = Join-Path $PSScriptRoot "../TakeCare/TakeCare.csproj"
$protectedConnection = Read-Host "Development SQL connection string (empty = local LocalDB)" -AsSecureString
$connection = [System.Net.NetworkCredential]::new("", $protectedConnection).Password
if ([string]::IsNullOrWhiteSpace($connection)) {
    $connection = "Server=(localdb)\MSSQLLocalDB;Database=TakeCARE_Dev;Integrated Security=true;Encrypt=true;TrustServerCertificate=true"
}
$bytes = [byte[]]::new(64)
$rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()
try { $rng.GetBytes($bytes) } finally { $rng.Dispose() }
$settings = @{
    "ConnectionStrings:defaultConnection" = $connection
    "Jwt:SigningKeyBase64" = [Convert]::ToBase64String($bytes)
    "Email:Enabled" = "false"
}
$settings | ConvertTo-Json -Compress | dotnet user-secrets set --project $project
if ($LASTEXITCODE -ne 0) { throw "Could not initialize development secrets." }
[Array]::Clear($bytes, 0, $bytes.Length)
$connection = $null
$settings.Clear()
Write-Host "Development configuration initialized. Existing JWT sessions are invalidated."
