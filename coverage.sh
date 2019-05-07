#!/bin/sh -eu

# function to display commands
exe() { echo; echo "\$ $*" ; "$@" ; }

# Parameters
framework="${1-netcoreapp2.1}"
config="${2-Debug}"

testResults="test/TestResults"
include="[Castle.Core.AsyncInterceptor]*"
exclude="[*.Tests]*"

# Cannot use a bash solution in alpine builds https://stackoverflow.com/a/246128
#rootDir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
rootDir=$(pwd)

testProj1="$rootDir/test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj"

# Restore the packages
exe dotnet restore "$rootDir"

# Build the test projects
exe dotnet build --no-restore -f "$framework" -c "$config" "$testProj1"

# Execute the tests
exe dotnet test --no-restore --no-build -f "$framework" -c "$config" \
"$testProj1" \
--results-directory "$rootDir/$testResults/output/" \
--logger "\"trx;LogFileName=$(basename "$testProj1" .csproj).trx\"" \
/p:CollectCoverage=true \
/p:Include="$include" \
/p:Exclude="$exclude" \
/p:CoverletOutput="$rootDir/$testResults/" \
/p:CoverletOutputFormat="\"json,opencover\""

# Install trx2junit if not already installed
if [ ! -f "$rootDir/$testResults/tools/trx2junit" ]
then
   exe dotnet tool install trx2junit --tool-path "$rootDir/$testResults/tools"
fi

# Install ReportGenerator if not already installed
if [ ! -f "$rootDir/$testResults/tools/reportgenerator" ]
then
   exe dotnet tool install dotnet-reportgenerator-globaltool --tool-path "$rootDir/$testResults/tools"
fi

# Convert the MSTest trx files to junit xml
exe "$rootDir/$testResults/tools/trx2junit" "$rootDir/$testResults/output"/*.trx

# Generate the reports
exe "$rootDir/$testResults/tools/reportgenerator" \
"-verbosity:Info" \
"-reports:$rootDir/$testResults/coverage.opencover.xml" \
"-targetdir:$rootDir/$testResults/Report" \
"-reporttypes:Html"
