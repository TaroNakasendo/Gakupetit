Param(
    [string]$PfxPath = "Gakupetit.Package\gakupetit-test.pfx",
    [string]$Subject = "CN=GakupetitTest",
    [int]$YearsValid = 10
)

Write-Host "This script creates a self-signed code signing certificate and exports it as a PFX for local MSIX signing."

$securePwd = Read-Host -AsSecureString "Enter password to protect the PFX (will not be displayed)"
if (-not $securePwd) {
    Write-Error "Password is required."
    exit 1
}

# Create certificate in CurrentUser\My store
$cert = New-SelfSignedCertificate -Type CodeSigningCert -Subject $Subject -CertStoreLocation "Cert:\CurrentUser\My" -KeyExportPolicy Exportable -KeyAlgorithm RSA -KeyLength 2048 -NotAfter (Get-Date).AddYears($YearsValid)

if (-not $cert) {
    Write-Error "Failed to create certificate."
    exit 1
}

# Ensure output directory exists
$dir = Split-Path -Path $PfxPath -Parent
if ($dir -and -not (Test-Path $dir)) {
    New-Item -ItemType Directory -Path $dir | Out-Null
}

# Export to PFX
Export-PfxCertificate -Cert $cert -FilePath $PfxPath -Password $securePwd -Force

if (Test-Path $PfxPath) {
    Write-Host "PFX exported to: $PfxPath"
    Write-Host "Add the PFX password to your build pipeline secrets if used in CI, and do NOT commit the PFX to source control."
} else {
    Write-Error "Failed to export PFX."
    exit 1
}