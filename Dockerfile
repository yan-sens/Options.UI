FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5191

ENV ASPNETCORE_URLS=http://+:5191

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Options.UI/Options.UI.csproj", "Options.UI/"]
RUN dotnet restore "Options.UI\Options.UI.csproj"
COPY . .
WORKDIR "/src/Options.UI"
RUN dotnet build "Options.UI.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Options.UI.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Options.UI.dll"]