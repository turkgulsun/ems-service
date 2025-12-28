FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/EmsService.API/EmsService.API.csproj", "src/EmsService.API/"]
COPY ["src/EmsService.Application/EmsService.Application.csproj", "src/EmsService.Application/"]
COPY ["src/EmsService.Domain/EmsService.Domain.csproj", "src/EmsService.Domain/"]
COPY ["src/EmsService.Infrastructure/EmsService.Infrastructure.csproj", "src/EmsService.Infrastructure/"]
RUN dotnet restore "src/EmsService.API/EmsService.API.csproj"
COPY . .
WORKDIR "/src/src/EmsService.API"
RUN dotnet build "EmsService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EmsService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmsService.API.dll"]

