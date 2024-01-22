FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["../ObjectStorageApp/ObjectStorageApp.csproj", "../ObjectStorageApp/"]
COPY ["../Shared/Shared.csproj", "../Shared/"]
RUN dotnet restore "../ObjectStorageApp/ObjectStorageApp.csproj"
COPY . .
WORKDIR "/src/../ObjectStorageApp"
RUN dotnet build "ObjectStorageApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ObjectStorageApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ObjectStorageApp.dll"]
