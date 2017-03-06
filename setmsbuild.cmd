:: Do nothing if the msbuild variable has been set.
@IF NOT ["%msbuild%"] == [""] GOTO :EOF

:: Use VS2017 if installed.
@IF ["%msbuild%"] == [""] (
    IF EXIST "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\Tools\VsMSBuildCmd.bat" (
        echo Setting up variables for Visual Studio 2017.
        echo call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\Tools\VsMSBuildCmd.bat"
        call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\Tools\VsMSBuildCmd.bat"
        SET msbuild=MSBuild.exe
    )
)

:: Use VS2015 if installed.
@IF ["%msbuild%"] == [""] (
    IF EXIST "%VS140COMNTOOLS%vsvars32.bat" (
        echo Setting up variables for Visual Studio 2015.
        call "%VS140COMNTOOLS%vsvars32.bat"
        SET msbuild=MSBuild.exe
    )
)

@IF ["%msbuild%"] == [""] (
    echo Unable to find MSBuild
    EXIT /B 2
)
