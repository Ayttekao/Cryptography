FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CourseWork/CourseWork.csproj", "CourseWork/"]
RUN dotnet restore "CourseWork/CourseWork.csproj"
COPY . .
WORKDIR "/src/CourseWork"
RUN dotnet build "CourseWork.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CourseWork.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CourseWork.dll"]
