FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PsyAssistPlatform.WebApi/PsyAssistPlatform.WebApi.csproj", "PsyAssistPlatform.WebApi/"]
RUN dotnet restore "PsyAssistPlatform.WebApi/PsyAssistPlatform.WebApi.csproj"
COPY ./ ./
RUN dotnet build "PsyAssistPlatform.WebApi/PsyAssistPlatform.WebApi.csproj" -c Release --restore
RUN dotnet publish "PsyAssistPlatform.WebApi/PsyAssistPlatform.WebApi.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "PsyAssistPlatform.WebApi.dll"]
