#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

#ENV DOTNET_URLS=http://+:5000;https://+:5001
WORKDIR /app

USER app

EXPOSE 8000
EXPOSE 8001


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Vendaval.Api/Vendaval.Api.csproj", "Vendaval.Api/"]
COPY ["Vendaval.Application/Vendaval.Application.csproj", "Vendaval.Application/"]
COPY ["Vendaval.Infrastructure/Vendaval.Infrastructure.csproj", "Vendaval.Infrastructure/"]
COPY ["Vendaval.Domain/Vendaval.Domain.csproj", "Vendaval.Domain/"]
RUN dotnet restore "./Vendaval.Api/./Vendaval.Api.csproj"
COPY . .
WORKDIR "/src/Vendaval.Api"
RUN dotnet build "./Vendaval.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Vendaval.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["Vendaval.Api/Vendaval.pfx", "./Vendaval.pfx"]
COPY ["Vendaval.Api/localhost.crt", "/usr/local/share/ca-certificates/localhost.crt"]
RUN update-ca-certificates
ENTRYPOINT ["dotnet", "Vendaval.Api.dll"]
