FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["./PaladinProject/PaladinProject.csproj", "./PaladinProject/"]

RUN dotnet restore "./PaladinProject/PaladinProject.csproj"
COPY . .

WORKDIR "/src/PaladinProject"
RUN dotnet build "PaladinProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaladinProject.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaladinProject.dll"]
