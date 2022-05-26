@echo off
SETLOCAL
SET Configuration=Release
if exist ".\bin\ArkDataProcessor-%Configuration%.7z" del .\bin\ArkDataProcessor-%Configuration%.7z
dotnet build src\ArkDataProcessor.sln -c %Configuration% -o .\bin\%Configuration%
if %ERRORLEVEL% NEQ 0 goto :end
7z a -t7z -r .\bin\ArkDataProcessor-%Configuration%.7z .\bin\%Configuration%
:end
ENDLOCAL