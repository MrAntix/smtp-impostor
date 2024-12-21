param($serviceName, $dist)
# Load the required assembly
Add-Type -AssemblyName System.DirectoryServices.AccountManagement

$exePath = Resolve-Path "$dist\SMTP.Impostor.Worker.exe"

$cred = Get-Credential

# Extract the username
$username = $cred.UserName

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
