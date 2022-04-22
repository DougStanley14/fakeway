#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5101

ENV ASPNETCORE_URLS=http://*:5101

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["microsvc-weather/microsvc-weather.csproj", "microsvc-weather/"]
RUN dotnet restore "microsvc-weather/microsvc-weather.csproj"
COPY . .
WORKDIR "/src/microsvc-weather"
RUN dotnet build "microsvc-weather.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "microsvc-weather.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "microsvc-weather.dll"]