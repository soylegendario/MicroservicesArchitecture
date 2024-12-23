FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS http://*:80
EXPOSE 80
EXPOSE 443
WORKDIR /src
COPY ["Inventory.Api/Inventory.Api.csproj", "Inventory.Api/"]
COPY ["Inventory.CrossCutting.Api.Authentication/Inventory.CrossCutting.Api.Authentication.csproj", "Inventory.CrossCutting.Api.Authentication/"]
COPY ["Inventory.CrossCutting.Api.HostedServices/Inventory.CrossCutting.Api.HostedServices.csproj", "Inventory.CrossCutting.Api.HostedServices/"]
COPY ["Inventory.Infrastructure.Contracts/Inventory.Infrastructure.Contracts.csproj", "Inventory.Infrastructure.Contracts/"]
COPY ["Inventory.CrossCutting.Data/Inventory.CrossCutting.Data.csproj", "Inventory.CrossCutting.Data/"]
COPY ["Inventory.Domain/Inventory.Domain.csproj", "Inventory.Domain/"]
COPY ["Inventory.Application/Inventory.Application.csproj", "Inventory.Application/"]
COPY ["Inventory.CrossCutting.Events/Inventory.CrossCutting.Events.csproj", "Inventory.CrossCutting.Events/"]
COPY ["Inventory.Infrastructure/Inventory.Infrastructure.csproj", "Inventory.Infrastructure/"]
COPY ["Inventory.CrossCutting.Cqrs/Inventory.CrossCutting.Cqrs.csproj", "Inventory.CrossCutting.Cqrs/"]
COPY ["Inventory.CrossCutting.Exceptions/Inventory.CrossCutting.Exceptions.csproj", "Inventory.CrossCutting.Exceptions/"]
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
