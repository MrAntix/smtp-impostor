param($serviceName)


$Path = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
Set-Location "$path\..\..\SMTP.Impostor.Web"

"Building $serviceName UI"

npm i
npm run build
