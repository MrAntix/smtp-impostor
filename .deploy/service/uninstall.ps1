param($serviceName)

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

    "========================================="
    "$serviceName not installed, no work done"
}