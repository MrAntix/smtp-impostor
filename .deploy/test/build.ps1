param($serviceName, $dist, $runtime)

"Building $serviceName $runtime"

Set-Location "$path\..\SMTP.Impostor.Test.Sender"

$output = "$dist\$runtime"
dotnet publish -c Release -o $output -r $runtime /p:CopyOutputSymbolsToPublishDirectory=false /p:PublishSingleFile=true
