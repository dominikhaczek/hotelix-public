#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
COPY nginx/id-local-hotelix.crt /usr/local/share/ca-certificates/id-local-hotelix.crt
COPY nginx/hotelix.crt /usr/local/share/ca-certificates/hotelix.crt
COPY nginx/seq-hotelix.crt /usr/local/share/ca-certificates/seq-hotelix.crt
RUN update-ca-certificates

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Hotelix.Client/Hotelix.Client.csproj", "Hotelix.Client/"]
RUN dotnet restore "Hotelix.Client/Hotelix.Client.csproj"
COPY . .
WORKDIR "/src/Hotelix.Client"
RUN dotnet build "Hotelix.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hotelix.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hotelix.Client.dll"]