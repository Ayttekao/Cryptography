FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Crypto2/Crypto2.csproj", "Crypto2/"]
RUN dotnet restore "Crypto2/Crypto2.csproj"
COPY . .
WORKDIR "/src/Crypto2"
RUN dotnet build "Crypto2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Crypto2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Crypto2.dll"]
