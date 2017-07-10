@SETLOCAL

@SET CACHED_NUGET="%USERPROFILE%\.nuget\NuGet.exe"

@IF NOT EXIST %CACHED_NUGET% (
	echo Downloading latest version of NuGet.exe...
	IF NOT EXIST "%USERPROFILE%\.nuget" md "%USERPROFILE%\.nuget"
	powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -OutFile %CACHED_NUGET:"='%"
)

@echo %CACHED_NUGET% restore castle.core.asyncinterceptor.sln
@%CACHED_NUGET% restore castle.core.asyncinterceptor.sln
