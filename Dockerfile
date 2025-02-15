FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Options.UI/OptionsStats.UI.csproj", "."]
RUN dotnet restore "OptionsStats.UI.csproj"
COPY . .
RUN dotnet build "OptionsStats.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OptionsStats.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OptionsStats.UI.dll"]
