@SETLOCAL

@SET nuget_exe=NuGet.exe

@IF ["%APPVEYOR%"] == [""] (
    call "%~dp0test\GetNuGet.cmd"
    SET nuget_exe="%~dp0test\.nuget\NuGet.exe"

    IF NOT EXIST "%~dp0test\.nuget\NuGet.exe" (
        echo Unable to find NuGet executable.
        EXIT /B 2
    )
)

::@echo %nuget_exe%

@echo %nuget_exe% restore "%~dp0test\packages.config" -PackagesDirectory "%~dp0test\packages"
@call %nuget_exe% restore "%~dp0test\packages.config" -PackagesDirectory "%~dp0test\packages"

@SET config=%1
@IF ["%config%"] == [""] (
   SET config=Release
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
   echo Unable to find MSpec console runner.
   EXIT /B 2
)
::@echo %xunit_exe%

@SET results_path=%~dp0test\TestResults
@SET test_assemblies=%~dp0test\Castle.Core.AsyncInterceptor.Tests\bin\%config%\net451\win7-x64\Castle.Core.AsyncInterceptor.Tests.dll
::@SET test_assemblies=%test_assemblies% <path to additional assembly>
@SET xunit_results=%results_path%\Xunit.Tests.html
@SET coverage_filter=+[Castle.Core.AsyncInterceptor*]* -[*.Tests]*
@SET coverage_results=%results_path%\Test.Coverage.xml

@IF NOT EXIST "%results_path%" MD "%results_path%"
::@echo "%xunit_exe%" %test_assemblies% -noshadow -html "%xunit_results%"
::@"%xunit_exe%" %test_assemblies% -noshadow -html "%xunit_results%"

@echo "%cover_exe%" -register:user "-target:%xunit_exe%" "-targetargs:%test_assemblies% -noshadow -html %xunit_results%" -returntargetcode -filter:^"%coverage_filter%^" "-output:%coverage_results%"
@"%cover_exe%" -register:user "-target:%xunit_exe%" "-targetargs:%test_assemblies% -noshadow -html %xunit_results%" -returntargetcode -filter:^"%coverage_filter%^" "-output:%coverage_results%"
@IF ERRORLEVEL 1 (
   echo Error executing the xunit tests
   EXIT /B 2
)

@echo "%report_exe%" -verbosity:Error "-reports:%coverage_results%" "-targetdir:%results_path%" -reporttypes:XmlSummary
@"%report_exe%" -verbosity:Error "-reports:%coverage_results%" "-targetdir:%results_path%" -reporttypes:XmlSummary

@echo "%report_exe%" -verbosity:Error "-reports:%coverage_results%" "-targetdir:%results_path%\Report" -reporttypes:Html
@"%report_exe%" -verbosity:Error "-reports:%coverage_results%" "-targetdir:%results_path%\Report" -reporttypes:Html
