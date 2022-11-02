################################################################################
# shellcheck
FROM koalaman/shellcheck-alpine:stable AS shellcheck

WORKDIR /source
COPY . .

# Run shell check on all the shell files.
RUN find . -type f -name '*.sh' | wc -l \
&& find . -type f -name '*.sh' | xargs shellcheck --external-sources --color=always

########################################################################################################################
# .NET 6
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS test

ENV CI=true

WORKDIR /source

# Copy just the solution and proj files to make best use of docker image caching.
COPY *.props *.sln ./
COPY ./src/Castle.Core.AsyncInterceptor/*.csproj ./src/Castle.Core.AsyncInterceptor/
COPY ./test/Castle.Core.AsyncInterceptor.Tests/*.csproj ./test/Castle.Core.AsyncInterceptor.Tests/

# Run restore on just the project files, this should cache the image after restore.
RUN dotnet restore

COPY . .

RUN ./coverage.sh net6.0 Debug
