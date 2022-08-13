FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000/tcp
EXPOSE 5000/udp
ENV ASPNETCORE_ENVIRONMENT DEVELOPMENT
ENV ASPNETCORE_URLS http://*:5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Vexis.API/Vexis.API.csproj", "Vexis.API/"]
COPY ["Vexis.API.Data/Vexis.API.Data.csproj", "Vexis.API.Data/"]
COPY ["Vexis.Database/Vexis.Database.csproj", "Vexis.Database/"]
COPY ["Vexis.Security/Vexis.Security.csproj", "Vexis.Security/"]
RUN dotnet restore "Vexis.API/Vexis.API.csproj"
COPY . .
WORKDIR "/src/Vexis.API"
RUN dotnet build "Vexis.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vexis.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vexis.API.dll"]
