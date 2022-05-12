FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Crypro1/Crypro1.csproj", "Crypro1/"]
RUN dotnet restore "Crypro1/Crypro1.csproj"
COPY . .
WORKDIR "/src/Crypro1"
RUN dotnet build "Crypro1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Crypro1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Crypro1.dll"]
