::dotnet xunit -framework net47 -configuration Debug -parallel none -diagnostics
dotnet test -f net47 --no-build --no-restore -c Debug -v detailed
