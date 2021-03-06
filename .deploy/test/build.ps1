param($serviceName, $dist, $runtime)

"Building $serviceName $runtime"

$path = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
Set-Location "$path\..\..\SMTP.Impostor.Test.Sender"

$output = "$dist\$runtime"
dotnet publish -c Release -o $output -r $runtime /p:CopyOutputSymbolsToPublishDirectory=false /p:PublishSingleFile=true
