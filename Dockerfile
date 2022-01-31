########################################################################################################################
# shellcheck - lining for bash scrips
FROM koalaman/shellcheck-alpine:stable

COPY . .

# Run shell check on all the shell files.
RUN find . -type f -name '*.sh' | wc -l find . -type f -name '*.sh' | xargs shellcheck --external-sources --color=always

########################################################################################################################
# .NET Core 3.1
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine

WORKDIR /work

# Copy just the solution and proj files to make best use of docker image caching.
COPY ./castle.core.asyncinterceptor.sln .
COPY ./src/Castle.Core.AsyncInterceptor/Castle.Core.AsyncInterceptor.csproj ./src/Castle.Core.AsyncInterceptor/Castle.Core.AsyncInterceptor.csproj
COPY ./test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj ./test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj

# Run restore on just the project files, this should cache the image after restore.
RUN dotnet restore

COPY . .

RUN ./coverage.sh netcoreapp3.1 Debug

########################################################################################################################
# .NET 5
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine

WORKDIR /work

# Copy just the solution and proj files to make best use of docker image caching.
COPY ./castle.core.asyncinterceptor.sln .
COPY ./src/Castle.Core.AsyncInterceptor/Castle.Core.AsyncInterceptor.csproj ./src/Castle.Core.AsyncInterceptor/Castle.Core.AsyncInterceptor.csproj
COPY ./test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj ./test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj

# Run restore on just the project files, this should cache the image after restore.
RUN dotnet restore

COPY . .

RUN ./coverage.sh net5.0 Debug

########################################################################################################################
# .NET 6
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine

WORKDIR /work

# Copy just the solution and proj files to make best use of docker image caching.
COPY ./castle.core.asyncinterceptor.sln .
COPY ./src/Castle.Core.AsyncInterceptor/Castle.Core.AsyncInterceptor.csproj ./src/Castle.Core.AsyncInterceptor/Castle.Core.AsyncInterceptor.csproj
COPY ./test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj ./test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj

# Run restore on just the project files, this should cache the image after restore.
RUN dotnet restore

COPY . .

RUN ./coverage.sh net6.0 Debug
