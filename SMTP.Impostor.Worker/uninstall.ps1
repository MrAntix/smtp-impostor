$ErrorActionPreference = "Stop"

if (-Not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] 'Administrator')) {
    "-----------------------------------------"
    "Service install required elevation"
    Pause
  
    $CommandLine = "-File `"" + $MyInvocation.MyCommand.Path + "`" " + $MyInvocation.UnboundArguments
    Start-Process -FilePath PowerShell.exe -Verb Runas -ArgumentList $CommandLine
    Exit
}

$serviceName = "SMTP Impostor"

try {

    $existingService = Get-WmiObject -Class Win32_Service -Filter "Name='$serviceName'"

    if ($existingService) 
    {
        "uninstalling '$serviceName'."
        Stop-Service $serviceName
        Start-Sleep -s 3
    
        $existingService.Delete() | Out-Null
        Start-Sleep -s 5

        "========================================="
        "$serviceName uninstalled"
    }
    else
    {
        "'$serviceName' service not found, nothing to do."
    }
  
} catch {

    "========================================="
    "Could not uninstall $serviceName"
    "$_.Exception"
}

Pause
