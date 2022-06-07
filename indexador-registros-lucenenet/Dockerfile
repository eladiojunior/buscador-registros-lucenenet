FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["indexador-registros-lucenenet/indexador-registros-lucenenet.csproj", "indexador-registros-lucenenet/"]
RUN dotnet restore "indexador-registros-lucenenet/indexador-registros-lucenenet.csproj"
COPY . .
WORKDIR "/src/indexador-registros-lucenenet"
RUN dotnet build "indexador-registros-lucenenet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "indexador-registros-lucenenet.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "indexador-registros-lucenenet.dll"]
