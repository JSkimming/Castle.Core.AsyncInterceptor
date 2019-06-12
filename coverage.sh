#!/bin/sh -eu

# function to display commands
exe() { echo; echo "\$ $*" ; "$@" ; }

# Parameters
framework="${1-netcoreapp2.1}"
config="${2-Debug}"

include="[Castle.Core.AsyncInterceptor]*"
exclude="[*.Tests]*"

# Cannot use a bash solution in alpine builds https://stackoverflow.com/a/246128
#rootDir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
rootDir=$(pwd)

testResults="test/TestResults"
output="$rootDir/$testResults/output"
tools="$rootDir/$testResults/tools"

testProj1="$rootDir/test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj"

# Restore the packages
exe dotnet restore "$rootDir"

# Build the test projects
exe dotnet build --no-restore -f "$framework" -c "$config" "$testProj1"

# Execute the tests
exe dotnet test --no-restore --no-build -f "$framework" -c "$config" \
"$testProj1" \
--results-directory "$output/" \
--logger "\"trx;LogFileName=$(basename "$testProj1" .csproj).trx\"" \
/p:CollectCoverage=true \
/p:Include="$include" \
/p:Exclude="$exclude" \
/p:CoverletOutput="$output/" \
/p:CoverletOutputFormat="\"json,opencover,cobertura\""

# Install trx2junit if not already installed
if [ ! -f "$tools/trx2junit" ]
then
   exe dotnet tool install trx2junit --tool-path "$tools"
fi

# Install ReportGenerator if not already installed
if [ ! -f "$tools/reportgenerator" ]
then
   exe dotnet tool install dotnet-reportgenerator-globaltool --tool-path "$tools"
fi

# Convert the MSTest trx files to junit xml
exe "$tools/trx2junit" "$output"/*.trx

# Generate the reports
exe "$tools/reportgenerator" \
"-verbosity:Info" \
"-reports:$output/coverage.opencover.xml" \
"-targetdir:$output/Report" \
"-reporttypes:Html"
