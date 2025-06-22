#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["src/Web/WebBff/WebBff.csproj", "src/Web/WebBff/"]
COPY ["src/Common/Common/Common.csproj", "src/Common/Common/"]
COPY ["src/Core/Core.Application/Core.Application.csproj", "src/Core/Core.Application/"]
COPY ["src/Core/Core.Domain/Core.Domain.csproj", "src/Core/Core.Domain/"]
COPY ["src/Core/Core.Shared/Core.Shared.csproj", "src/Core/Core.Shared/"]
COPY ["src/Core/Core.Endpoints/Core.Endpoints.csproj", "src/Core/Core.Endpoints/"]
COPY ["src/Core/Core.Infrastructure/Core.Infrastructure.csproj", "src/Core/Core.Infrastructure/"]
COPY ["src/Modules/Student/Student.Infrastructure/Student.Infrastructure.csproj", "src/Modules/Student/Student.Infrastructure/"]
COPY ["src/Modules/Student/Student.Application/Student.Application.csproj", "src/Modules/Student/Student.Application/"]
COPY ["src/Modules/Subject/Subject.Shared/Subject.Shared.csproj", "src/Modules/Subject/Subject.Shared/"]
COPY ["src/Modules/Subject/Subject.Domain/Subject.Domain.csproj", "src/Modules/Subject/Subject.Domain/"]
COPY ["src/Modules/Student/Student.Domain/Student.Domain.csproj", "src/Modules/Student/Student.Domain/"]
COPY ["src/Modules/Student/Student.Persistence/Student.Persistence.csproj", "src/Modules/Student/Student.Persistence/"]
COPY ["src/Core/Core.Persistence/Core.Persistence.csproj", "src/Core/Core.Persistence/"]
COPY ["src/Modules/Student/Student.Shared/Student.Shared.csproj", "src/Modules/Student/Student.Shared/"]
COPY ["src/Modules/Subject/Subject.Infrastructure/Subject.Infrastructure.csproj", "src/Modules/Subject/Subject.Infrastructure/"]
COPY ["src/Modules/Subject/Subject.Application/Subject.Application.csproj", "src/Modules/Subject/Subject.Application/"]
COPY ["src/Modules/Subject/Subject.Persistence/Subject.Persistence.csproj", "src/Modules/Subject/Subject.Persistence/"]
RUN dotnet restore "./src/Web/WebBff/./WebBff.csproj"
COPY . .
WORKDIR "/src/src/Web/WebBff"
RUN dotnet build "./WebBff.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebBff.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

USER root
RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
    && echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
    && wget https://download.newrelic.com/548C16BF.gpg \
    && apt-key add 548C16BF.gpg \
    && apt-get update \
    && apt-get install -y 'newrelic-dotnet-agent' \
    && rm -rf /var/lib/apt/lists/*

ENV CORECLR_ENABLE_PROFILING=1 \
    CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
    CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
    CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
    NEW_RELIC_LICENSE_KEY=fcc95df14279e9c71621a585b54b7d13FFFFNRAL \
    NEW_RELIC_APP_NAME="gallilearn"

WORKDIR /app
COPY --from=publish /app/publish .

USER app
ENTRYPOINT ["dotnet", "WebBff.dll"]
