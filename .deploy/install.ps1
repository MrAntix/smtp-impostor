$serviceName = "SMTP Impostor"
$path = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
$dist = "$path\..\dist"

# elevate if needed
if (-Not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] 'Administrator')) {
    "-----------------------------------------"
    "Service install required elevation"
  
    $file = $MyInvocation.MyCommand.Path
  
    Start-Process -FilePath PowerShell.exe -Verb Runas -ArgumentList "-Command `"cd $path; & $file`""
    Exit
}

try {

    $existingService = Get-WmiObject -Class Win32_Service -Filter "Name='$serviceName'"

    if ($existingService)
    {
        & "$path\service\uninstall.ps1" $serviceName
    }

    & "$path\ui\build.ps1" $serviceName
    & "$path\service\build.ps1" $serviceName $dist "win-x64"
    & "$path\service\install.ps1" $serviceName "$dist\win-x64"
    

    "========================================="
    "$serviceName installed"

} catch {

    "========================================="
    "Could not install $serviceName"
    "$_.Exception"

}

Pause