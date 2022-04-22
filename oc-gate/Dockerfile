#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5100
# EXPOSE 5101
# EXPOSE 5102

ENV ASPNETCORE_URLS=http://*:5100

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["oc-gate/oc-gate.csproj", "oc-gate/"]
RUN dotnet restore "oc-gate/oc-gate.csproj"
COPY . .
WORKDIR "/src/oc-gate"
RUN dotnet build "oc-gate.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "oc-gate.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "oc-gate.dll"]