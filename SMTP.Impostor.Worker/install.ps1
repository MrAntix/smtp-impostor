$ErrorActionPreference = "Stop"

# elevate if needed
if (-Not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] 'Administrator')) {
    "-----------------------------------------"
    "Service install required elevation"
    Pause
  
    $wd = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
    $file = $MyInvocation.MyCommand.Path
  
    Start-Process -FilePath PowerShell.exe -Verb Runas -ArgumentList "-Command `"cd $wd; & $file`""
    Exit
}

try {

    $cred = Get-Credential -Message "Log on user for SMTP Impostor Service"

    $serviceName = "SMTP Impostor"
    $existingService = Get-WmiObject -Class Win32_Service -Filter "Name='$serviceName'"

    if ($existingService)
    {
      "'$serviceName' exists, uninstalling old service"
      Stop-Service $serviceName
      Start-Sleep -s 3
      $existingService.Delete() | Out-Null
      Start-Sleep -s 5
    }

    "Building UI"
    npm i
    npm run build

    "Building Worker"
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

    "Starting the service."
    Start-Service $serviceName

    "========================================="
    "$serviceName installed"

} catch {

    "========================================="
    "Could not install $serviceName"
    "$_.Exception"

}

Pause