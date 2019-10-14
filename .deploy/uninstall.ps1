$serviceName = "SMTP Impostor"
$path = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent

if (-Not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] 'Administrator')) {
    "-----------------------------------------"
    "Service install required elevation"

    $CommandLine = "-File `"" + $MyInvocation.MyCommand.Path + "`""
    Start-Process -FilePath PowerShell.exe -Verb Runas -ArgumentList $CommandLine
    Exit
}

try {

    & "$path\service\uninstall.ps1" $serviceName

} catch {

    "========================================="
    "Could not install $serviceName"
    "$_.Exception"
}

Pause