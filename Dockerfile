#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BankingSystem/BankingSystem.WebApi.csproj", "BankingSystem/"]
RUN dotnet restore "BankingSystem/BankingSystem.WebApi.csproj"
COPY . .
WORKDIR "/src/BankingSystem"
RUN dotnet build "BankingSystem.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankingSystem.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankingSystem.WebApi.dll"]