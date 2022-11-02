#!/bin/sh -eu

# function to display commands
exe() { echo; echo "\$ $*" ; "$@" ; }

# Parameters
framework="${1-net7.0}"
config="${2-Debug}"

include="[Castle.Core.AsyncInterceptor]*"
exclude="\"[Castle.Core.AsyncInterceptor]*.NoCoverage.*,[*.Tests]*\""

# Cannot use a bash solution in alpine builds https://stackoverflow.com/a/246128
#rootDir="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
rootDir=$(pwd)

testResults="test/TestResults"
output="$rootDir/$testResults/output"

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
--logger "\"html;LogFileName=$(basename "$testProj1" .csproj).html\"" \
-p:CollectCoverage=true \
-p:Include="$include" \
-p:Exclude="$exclude" \
-p:CoverletOutput="$output/" \
-p:CoverletOutputFormat="\"json,opencover,cobertura,lcov\""

exe dotnet tool restore

# Convert the MSTest trx files to junit xml
exe dotnet tool run trx2junit "$output"/*.trx

# Generate the reports
exe dotnet tool run reportgenerator \
"-verbosity:Info" \
"-reports:$output/coverage.$framework.opencover.xml" \
"-targetdir:$output/Report" \
"-reporttypes:Html"
