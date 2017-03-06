@SETLOCAL

@SET CACHED_NUGET="%USERPROFILE%\.nuget\NuGet.exe"
@SET LOCAL_NUGET="%~dp0.nuget\NuGet.exe"

@IF EXIST %CACHED_NUGET% goto copynuget
@echo Downloading latest version of NuGet.exe...
@IF NOT EXIST "%USERPROFILE%\.nuget" md "%USERPROFILE%\.nuget"
powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://dist.nuget.org/win-x86-commandline/v4.0.0/nuget.exe' -OutFile %CACHED_NUGET:"='%"

:copynuget
@IF EXIST %LOCAL_NUGET% goto restore
@IF NOT EXIST "%~dp0.nuget" md "%~dp0.nuget"
copy %CACHED_NUGET% %LOCAL_NUGET% > nul

:restore
%LOCAL_NUGET% restore
