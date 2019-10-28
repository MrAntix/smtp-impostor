$serviceName = "SMTP Impostor"
$path = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
$dist = "$path\..\dist"

try {
    Remove-Item $dist -Force -Recurse -ErrorAction Ignore

    & "$path\test\build.ps1" $serviceName "$dist\tester" "win-x64"
    & "$path\ui\build.ps1" $serviceName
    & "$path\service\build.ps1" $serviceName "$dist\worker" "win-x64"
    
    "Done."

} catch {

    Write-Host "ERROR =============================`r`n",
        "Could not build $serviceName`r`n",
        "$_.Exception"     -ForegroundColor Red

}

Set-Location $path