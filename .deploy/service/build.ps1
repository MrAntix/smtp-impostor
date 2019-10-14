param($serviceName, $dist, $runtime)

"Building $serviceName $runtime"

$path = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
Set-Location "$path\..\..\SMTP.Impostor.Worker"

$output = "$dist\$runtime"
dotnet publish -c Release -o $output -r $runtime /p:CopyOutputSymbolsToPublishDirectory=false
# NB  /p:PublishSingleFile=true stops config from loading https://github.com/dotnet/core-setup/issues/7491
