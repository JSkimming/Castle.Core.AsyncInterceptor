########################################################################################################################
# shellcheck - lining for bash scrips
FROM nlknguyen/alpine-shellcheck:v0.4.6

COPY ./ ./

# Convert CRLF to CR as it causes shellcheck warnings.
RUN find . -type f -name '*.sh' -exec dos2unix {} \;

# Run shell check on all the shell files.
RUN find . -type f -name '*.sh' | wc -l && find . -type f -name '*.sh' | xargs shellcheck --external-sources

########################################################################################################################
# .NET Core 1.1
FROM mcr.microsoft.com/dotnet/core/sdk:1.1

ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true

WORKDIR /work

# Copy just the solution and proj files to make best use of docker image caching.
COPY ./castle.core.asyncinterceptor.sln .
COPY ./src/Castle.Core.AsyncInterceptor/Castle.Core.AsyncInterceptor.csproj ./src/Castle.Core.AsyncInterceptor/Castle.Core.AsyncInterceptor.csproj
COPY ./test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj ./test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj

# Run restore on just the project files, this should cache the image after restore.
RUN dotnet restore

COPY . .

# Build to ensure the tests are their own distinct step
RUN dotnet build -f netcoreapp1.1 -c Debug ./test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj

# Run unit tests
RUN dotnet test --no-build -f netcoreapp1.1 -c Debug ./test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj

########################################################################################################################
# .NET Core 2.1
FROM mcr.microsoft.com/dotnet/core/sdk:2.1-alpine

ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true

RUN apk add --update dos2unix && rm -rf /var/cache/apk/*

WORKDIR /work

# Copy just the solution and proj files to make best use of docker image caching.
COPY ./castle.core.asyncinterceptor.sln .
COPY ./src/Castle.Core.AsyncInterceptor/Castle.Core.AsyncInterceptor.csproj ./src/Castle.Core.AsyncInterceptor/Castle.Core.AsyncInterceptor.csproj
COPY ./test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj ./test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj

# Run restore on just the project files, this should cache the image after restore.
RUN dotnet restore

COPY . .

RUN dos2unix -k -q ./coverage.sh

RUN ./coverage.sh netcoreapp2.1 Debug

########################################################################################################################
# .NET Core 2.2
FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine

ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true

RUN apk add --update dos2unix && rm -rf /var/cache/apk/*

WORKDIR /work

# Copy just the solution and proj files to make best use of docker image caching.
COPY ./castle.core.asyncinterceptor.sln .
COPY ./src/Castle.Core.AsyncInterceptor/Castle.Core.AsyncInterceptor.csproj ./src/Castle.Core.AsyncInterceptor/Castle.Core.AsyncInterceptor.csproj
COPY ./test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj ./test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj

# Run restore on just the project files, this should cache the image after restore.
RUN dotnet restore

COPY . .

RUN dos2unix -k -q ./coverage.sh

RUN ./coverage.sh netcoreapp2.1 Debug
