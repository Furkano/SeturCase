FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["RaportApi/RaportApi.csproj", "RaportApi/"]
RUN dotnet restore "RaportApi/RaportApi.csproj"
COPY . .
WORKDIR "/src/RaportApi"
RUN dotnet build "RaportApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RaportApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RaportApi.dll"]