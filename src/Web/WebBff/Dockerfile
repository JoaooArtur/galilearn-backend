FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS base
WORKDIR /app

EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

ARG NUGET_USER
ARG NUGET_PACKAGES_TOKEN
ENV NUGET_USER=$NUGET_USER
ENV NUGET_PACKAGES_TOKEN=$NUGET_PACKAGES_TOKEN

WORKDIR /src

COPY ["Directory.Packages.props", "."]

COPY ["src/Web/WebBff/WebBff.csproj", "src/Web/WebBff/"]
COPY ["src/Common/Common/Common.csproj", "src/Common/Common/"]
COPY ["src/Core/Core.Application/Core.Application.csproj", "src/Core/Core.Application/"]
COPY ["src/Core/Core.Domain/Core.Domain.csproj", "src/Core/Core.Domain/"]
COPY ["src/Core/Core.Shared/Core.Shared.csproj", "src/Core/Core.Shared/"]
COPY ["src/Core/Core.Infrastructure/Core.Infrastructure.csproj", "src/Core/Core.Infrastructure/"]
COPY ["src/Core/Core.Persistence/Core.Persistence.csproj", "src/Core/Core.Persistence/"]
COPY ["src/Core/Core.Endpoints/Core.Endpoints.csproj", "src/Core/Core.Endpoints/"]
COPY ["src/Modules/Student/Student.Infrastructure/Student.Infrastructure.csproj", "src/Modules/Student/Student.Infrastructure/"]
COPY ["src/Modules/Student/Student.Application/Student.Application.csproj", "src/Modules/Student/Student.Application/"]
COPY ["src/Modules/Student/Student.Persistence/Student.Persistence.csproj", "src/Modules/Student/Student.Persistence/"]
COPY ["src/Modules/Student/Student.Shared/Student.Shared.csproj", "src/Modules/Student/Student.Shared/"]
COPY ["src/Modules/Student/Student.Domain/Student.Domain.csproj", "src/Modules/Student/Student.Domain/"]
COPY ["src/Modules/Subject/Subject.Infrastructure/Subject.Infrastructure.csproj", "src/Modules/Subject/Subject.Infrastructure/"]
COPY ["src/Modules/Subject/Subject.Application/Subject.Application.csproj", "src/Modules/Subject/Subject.Application/"]
COPY ["src/Modules/Subject/Subject.Persistence/Subject.Persistence.csproj", "src/Modules/Subject/Subject.Persistence/"]
COPY ["src/Modules/Subject/Subject.Shared/Subject.Shared.csproj", "src/Modules/Subject/Subject.Shared/"]
COPY ["src/Modules/Subject/Subject.Domain/Subject.Domain.csproj", "src/Modules/Subject/Subject.Domain/"]

RUN dotnet restore "./src/Web/WebBff/WebBff.csproj"
COPY . .
WORKDIR "/src/src/Web/WebBff"
RUN dotnet build "./WebBff.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./WebBff.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final

RUN apk add --no-cache icu-libs
RUN apk add --no-cache icu-data-full
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    TZ=America/Sao_Paulo
ENV LC_ALL=pt_BR.UTF-8
ENV LANG=pt_BR.UTF-8

WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebBff.dll"]