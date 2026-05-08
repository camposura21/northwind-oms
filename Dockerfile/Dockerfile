# Backend
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5164

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Northwind.API/Northwind.API.csproj", "Northwind.API/"]
COPY ["Northwind.Application/Northwind.Application.csproj", "Northwind.Application/"]
COPY ["Northwind.Domain/Northwind.Domain.csproj", "Northwind.Domain/"]
COPY ["Northwind.Infrastructure/Northwind.Infrastructure.csproj", "Northwind.Infrastructure/"]
RUN dotnet restore "Northwind.API/Northwind.API.csproj"
COPY . .
WORKDIR "/src/Northwind.API"
RUN dotnet build "Northwind.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Northwind.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Northwind.API.dll"]