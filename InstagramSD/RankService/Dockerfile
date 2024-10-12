#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["RankService/RankService.csproj", "RankService/"]
COPY ["InstagramSD/Common/Common.csproj", "Common/"]
COPY ["InstagramSD/Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "RankService/RankService.csproj"
COPY . .
WORKDIR "/src/RankService"
RUN dotnet build "RankService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RankService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RankService.dll"]
