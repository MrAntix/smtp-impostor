$ErrorActionPreference = "Stop"

$serviceName = "SMTP Impostor"

$existingService = Get-WmiObject -Class Win32_Service -Filter "Name='$serviceName'"

if ($existingService) 
{
  "stopping '$serviceName'."
  Stop-Service $serviceName
  Start-Sleep -s 3
    
  "uninstalling '$serviceName'."
  $existingService.Delete()
  Start-Sleep -s 5

  "Completed."
}
else
{
  "'$serviceName' service not found, nothing to do."
}

