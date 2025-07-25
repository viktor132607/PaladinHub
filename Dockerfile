FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["./PaladinHub/PaladinHub.csproj", "./PaladinHub/"]

RUN dotnet restore "./PaladinHub/PaladinHub.csproj"
COPY . .

WORKDIR "/src/PaladinHub"
RUN dotnet build "PaladinHub.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaladinHub.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaladinHub.dll"]
