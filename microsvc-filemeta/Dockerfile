#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5102

ENV ASPNETCORE_URLS=http://*:5102

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["microsvc-filemeta/microsvc-filemeta.csproj", "microsvc-filemeta/"]
RUN dotnet restore "microsvc-filemeta/microsvc-filemeta.csproj"
COPY . .
WORKDIR "/src/microsvc-filemeta"
RUN dotnet build "microsvc-filemeta.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "microsvc-filemeta.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "microsvc-filemeta.dll"]