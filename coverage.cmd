@SETLOCAL

@SET NUGET_EXE=NuGet
@SET CACHED_NUGET="%USERPROFILE%\.nuget\NuGet.exe"

@IF ["%APPVEYOR%"] == [""] (
    SET NUGET_EXE=%CACHED_NUGET%

    IF NOT EXIST %CACHED_NUGET% (
        echo Downloading latest version of NuGet.exe...
        IF NOT EXIST "%USERPROFILE%\.nuget" md "%USERPROFILE%\.nuget"
        powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -OutFile %CACHED_NUGET:"='%"
    )
)

::@echo %NUGET_EXE%

@echo %NUGET_EXE% restore "%~dp0test\packages.config" -PackagesDirectory "%~dp0test\packages"
@call %NUGET_EXE% restore "%~dp0test\packages.config" -PackagesDirectory "%~dp0test\packages"

@SET config=%1
@IF ["%config%"] == [""] (
   SET config=Debug
)

@FOR /r %%F IN (*OpenCover.Console.exe) DO @SET cover_exe=%%F
@IF NOT EXIST "%cover_exe%" (
   echo Unable to find OpenCover console.
   EXIT /B 2
)
::@echo %cover_exe%

@FOR /r %%F IN (*ReportGenerator.exe) DO @SET report_exe=%%F
@IF NOT EXIST "%report_exe%" (
   echo Unable to find ReportGenerator.
   EXIT /B 2
)
::@echo %report_exe%

@SET results_path=%~dp0test\TestResults
@SET xunit_results=%results_path%\Xunit.Tests.html
@SET coverage_filter=+[Castle.Core.AsyncInterceptor*]* -[*.Tests]*
@SET coverage_results=%results_path%\Test.Coverage.xml

@IF NOT EXIST "%results_path%" MD "%results_path%"

@echo dotnet build --configuration %config% "%~dp0test\Castle.Core.AsyncInterceptor.Tests\Castle.Core.AsyncInterceptor.Tests.csproj"
@dotnet build --configuration %config% "%~dp0test\Castle.Core.AsyncInterceptor.Tests\Castle.Core.AsyncInterceptor.Tests.csproj"
@IF ERRORLEVEL 1 (
   echo Error building the test project
   EXIT /B 2
)

cd "%~dp0test\Castle.Core.AsyncInterceptor.Tests"

::@echo dotnet.exe xunit -framework net47 -configuration %config% -nobuild -noshadow -html %xunit_results%
::@dotnet.exe xunit -framework net47 -configuration %config% -nobuild -noshadow -html %xunit_results%

@echo "%cover_exe%" -register:user "-target:dotnet.exe" "-targetargs:xunit -framework net47 -configuration %config% -nobuild -noshadow -html %xunit_results%" -returntargetcode -filter:^"%coverage_filter%^" "-output:%coverage_results%"
@"%cover_exe%" -register:user "-target:dotnet.exe" "-targetargs:xunit -framework net47 -configuration %config% -nobuild -noshadow -html %xunit_results%" -returntargetcode -filter:^"%coverage_filter%^" "-output:%coverage_results%"
@IF ERRORLEVEL 1 (
   echo Error executing the xunit tests
   EXIT /B 2
)

cd "%~dp0"

@echo "%report_exe%" -verbosity:Error "-reports:%coverage_results%" "-targetdir:%results_path%\Report" -reporttypes:Html
@"%report_exe%" -verbosity:Error "-reports:%coverage_results%" "-targetdir:%results_path%\Report" -reporttypes:Html
