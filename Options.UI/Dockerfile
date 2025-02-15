FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5153

ENV ASPNETCORE_URLS=http://+:5153

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["OptionsStats.UI/OptionsStats.UI.csproj", "OptionsStats.UI/"]
RUN dotnet restore "OptionsStats.UI\OptionsStats.UI.csproj"
COPY . .
WORKDIR "/src/OptionsStats.UI"
RUN dotnet build "OptionsStats.UI.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "OptionsStats.UI.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OptionsStats.UI.dll"]
