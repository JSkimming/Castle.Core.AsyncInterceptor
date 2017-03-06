@call "%~dp0setmsbuild.cmd"

@SETLOCAL

@SET config=%1
@IF ["%config%"] == [""] (
   SET config=Release
)

@echo msbuild "%~dp0src\Castle.Core.AsyncInterceptor\Castle.Core.AsyncInterceptor.csproj" /nologo /verbosity:m /t:pack /p:Configuration=%config%;PackageVersion=0.1.2-JustTesting;IncludeSource=true
@msbuild "%~dp0src\Castle.Core.AsyncInterceptor\Castle.Core.AsyncInterceptor.csproj" /nologo /verbosity:m /t:pack /p:Configuration=%config%;PackageVersion=0.1.2-JustTesting;IncludeSource=true
