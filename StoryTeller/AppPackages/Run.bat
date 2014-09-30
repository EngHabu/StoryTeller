pushd %~dp0
Powershell.exe -Command "Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy Unrestricted"
Powershell.exe -File Run.ps1 -ExecutionPolicy Unrestricted
popd