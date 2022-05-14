FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Crypto3/Crypto3.csproj", "Crypto3/"]
RUN dotnet restore "Crypto3/Crypto3.csproj"
COPY . .
WORKDIR "/src/Crypto3"
RUN dotnet build "Crypto3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Crypto3.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Crypto3.dll"]
