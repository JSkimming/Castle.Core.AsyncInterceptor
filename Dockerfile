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
RUN dotnet test --no-build -c Debug -f netcoreapp1.1 test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj

########################################################################################################################
# .NET Core 2.1
FROM mcr.microsoft.com/dotnet/core/sdk:2.1-alpine

ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true

WORKDIR /work

# Copy just the solution and proj files to make best use of docker image caching.
COPY ./castle.core.asyncinterceptor.sln .
COPY ./src/Castle.Core.AsyncInterceptor/Castle.Core.AsyncInterceptor.csproj ./src/Castle.Core.AsyncInterceptor/Castle.Core.AsyncInterceptor.csproj
COPY ./test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj ./test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj

# Run restore on just the project files, this should cache the image after restore.
RUN dotnet restore

COPY . .

RUN ./coverage.sh netcoreapp2.1 Debug

########################################################################################################################
# .NET Core 2.2
FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine

ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true

WORKDIR /work

# Copy just the solution and proj files to make best use of docker image caching.
COPY ./castle.core.asyncinterceptor.sln .
COPY ./src/Castle.Core.AsyncInterceptor/Castle.Core.AsyncInterceptor.csproj ./src/Castle.Core.AsyncInterceptor/Castle.Core.AsyncInterceptor.csproj
COPY ./test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj ./test/Castle.Core.AsyncInterceptor.Tests/Castle.Core.AsyncInterceptor.Tests.csproj

# Run restore on just the project files, this should cache the image after restore.
RUN dotnet restore

COPY . .

RUN ./coverage.sh netcoreapp2.1 Debug
