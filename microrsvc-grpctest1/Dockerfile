#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["microrsvc-grpctest1/microrsvc-grpctest1.csproj", "microrsvc-grpctest1/"]
RUN dotnet restore "microrsvc-grpctest1/microrsvc-grpctest1.csproj"
COPY . .
WORKDIR "/src/microrsvc-grpctest1"
RUN dotnet build "microrsvc-grpctest1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "microrsvc-grpctest1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "microrsvc-grpctest1.dll"]