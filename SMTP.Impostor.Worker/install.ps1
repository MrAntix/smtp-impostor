# https://github.com/philoushka/blog/blob/master/install-a-windows-service-using-powershell.md

$ErrorActionPreference = "Stop"

$cred = Get-Credential -Message "Log on user for SMTP Impostor Service"

$serviceName = "SMTP Impostor"
$existingService = Get-WmiObject -Class Win32_Service -Filter "Name='$serviceName'"

if ($existingService)
{
  "'$serviceName' exists already. Stopping."
  Stop-Service $serviceName
  "Waiting 3 seconds to allow existing service to stop."
  Start-Sleep -s 3

  $existingService.Delete()
  "Waiting 5 seconds to allow service to be uninstalled."
  Start-Sleep -s 5
}

npm i
npm run build

dotnet publish -c Release

$exePath = Resolve-Path ".\bin\Release\netcoreapp3.0\publish\SMTP.Impostor.Worker.exe"
$username = ($cred.Domain + "\" + $cred.UserName).TrimStart("\");
"Setting access for '$username'"

$acl = Get-Acl $exePath
$permission = $username, "FullControl", "Allow"
$accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule $permission
$acl.SetAccessRule($accessRule)
Set-Acl -Path $exePath -AclObject $acl

"Installing the service."
New-Service -BinaryPathName $exePath -Name $serviceName -Credential $cred -DisplayName $serviceName -StartupType Automatic
"Installed the service."

"Starting the service."
Start-Service $serviceName

"Completed."
