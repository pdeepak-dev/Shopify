#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Micros/Order/Order.API/Order.API.csproj", "Micros/Order/Order.API/"]
COPY ["Micros/Order/Order.Infrastructure/Order.Infrastructure.csproj", "Micros/Order/Order.Infrastructure/"]
COPY ["Micros/Order/Core/Order.Application/Order.Application.csproj", "Micros/Order/Core/Order.Application/"]
COPY ["Micros/Order/Core/Order.Domain/Order.Domain.csproj", "Micros/Order/Core/Order.Domain/"]
RUN dotnet restore "Micros/Order/Order.API/Order.API.csproj"
COPY . .
WORKDIR "/src/Micros/Order/Order.API"
RUN dotnet build "Order.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Order.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.API.dll"]
