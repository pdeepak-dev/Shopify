#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Micros/Catalog/Catalog.API/Catalog.API.csproj", "Micros/Catalog/Catalog.API/"]
RUN dotnet clean "Micros/Catalog/Catalog.API/Catalog.API.csproj"
RUN dotnet restore "Micros/Catalog/Catalog.API/Catalog.API.csproj"
COPY . .
WORKDIR "/src/Micros/Catalog/Catalog.API"
RUN dotnet build "Catalog.API.csproj" -c Release -f net5.0 -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.API.csproj" -c Release -f net5.0 -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]