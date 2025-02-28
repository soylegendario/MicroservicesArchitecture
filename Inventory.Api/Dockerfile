FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS http://*:80
EXPOSE 80
EXPOSE 443
WORKDIR /src

ENV DOTNET_USE_POLLING_FILE_WATCHER 1
ENV DOTNET_HOST_PATH /usr/share/dotnet/dotnet
ENV DOTNET_RUNNING_IN_CONTAINER true
# Habilita el servidor de depuración
ENV DOTNET_MODIFIABLE_ASSEMBLIES debug
EXPOSE 5000 5001 5004 9229

COPY ["Inventory.Api/Inventory.Api.csproj", "Inventory.Api/"]
COPY ["CrossCutting.Api.Authentication/CrossCutting.Api.Authentication.csproj", "CrossCutting.Api.Authentication/"]
COPY ["CrossCutting.Cqrs/CrossCutting.Cqrs.csproj", "CrossCutting.Cqrs/"]
COPY ["CrossCutting.Data/CrossCutting.Data.csproj", "CrossCutting.Data/"]
COPY ["Inventory.Domain/Inventory.Domain.csproj", "Inventory.Domain/"]
COPY ["CrossCutting.Exceptions/CrossCutting.Exceptions.csproj", "CrossCutting.Exceptions/"]
COPY ["Inventory.Application/Inventory.Application.csproj", "Inventory.Application/"]
COPY ["Inventory.Infrastructure/Inventory.Infrastructure.csproj", "Inventory.Infrastructure/"]
RUN dotnet restore "Inventory.Api/Inventory.Api.csproj"
COPY . .
WORKDIR "/src/Inventory.Api"
RUN dotnet build "Inventory.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inventory.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inventory.Api.dll"]
