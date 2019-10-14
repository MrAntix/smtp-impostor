param($serviceName, $dist)

$exePath = Resolve-Path "$dist\SMTP.Impostor.Worker.exe"

$cred = Get-Credential -Message "Log on user for SMTP Impostor Service"
$username = ($cred.Domain + "\" + $cred.UserName).TrimStart("\");
"Setting access for '$username'"

$acl = Get-Acl $exePath
$permission = $username, "FullControl", "Allow"
$accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule $permission
$acl.SetAccessRule($accessRule)
Set-Acl -Path $exePath -AclObject $acl

"Installing the service."
New-Service -BinaryPathName $exePath -Name $serviceName -Credential $cred -DisplayName $serviceName -StartupType Automatic

"Starting the service."
Start-Service $serviceName
