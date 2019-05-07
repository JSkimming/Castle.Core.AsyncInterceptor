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

@FOR /r %%F IN (*xunit.console.exe) DO @SET xunit_exe=%%F
@IF NOT EXIST "%xunit_exe%" (
   echo Unable to find xUnit console runner.
   EXIT /B 2
)
::@echo %xunit_exe%

@SET results_path=%~dp0test\TestResults
@SET test_assemblies=%~dp0test\Castle.Core.AsyncInterceptor.Tests\bin\%config%\net472\Castle.Core.AsyncInterceptor.Tests.dll
::@SET test_assemblies=%test_assemblies% %~dp0test\More.Tests\bin\%config%\net472\More.Tests.dll
@SET xunit_results=%results_path%\Xunit.Tests.html
@SET coverage_filter=+[Castle.Core.AsyncInterceptor*]* -[*.Tests]*
@SET coverage_results=%results_path%\Test.Coverage.xml

@IF NOT EXIST "%results_path%" MD "%results_path%"

@echo dotnet build --framework net472 --configuration %config% "%~dp0test\Castle.Core.AsyncInterceptor.Tests\Castle.Core.AsyncInterceptor.Tests.csproj"
@dotnet build --framework net472 --configuration %config% "%~dp0test\Castle.Core.AsyncInterceptor.Tests\Castle.Core.AsyncInterceptor.Tests.csproj"
@IF ERRORLEVEL 1 (
   echo Error building the test project
   EXIT /B 2
)

::@echo "%xunit_exe%" %test_assemblies% -noshadow -html "%xunit_results%"
::@"%xunit_exe%" %test_assemblies% -noshadow -html "%xunit_results%"

@echo "%cover_exe%" -register:user "-target:%xunit_exe%" "-targetargs:%test_assemblies% -noshadow -html %xunit_results%" -returntargetcode -filter:^"%coverage_filter%^" "-output:%coverage_results%"
@"%cover_exe%" -register:user "-target:%xunit_exe%" "-targetargs:%test_assemblies% -noshadow -html %xunit_results%" -returntargetcode -filter:^"%coverage_filter%^" "-output:%coverage_results%"
@IF ERRORLEVEL 1 (
   echo Error executing the xunit tests
   EXIT /B 2
)

@echo "%report_exe%" -verbosity:Info "-reports:%coverage_results%" "-targetdir:%results_path%\Report" -reporttypes:Html
@"%report_exe%" -verbosity:Info "-reports:%coverage_results%" "-targetdir:%results_path%\Report" -reporttypes:Html
