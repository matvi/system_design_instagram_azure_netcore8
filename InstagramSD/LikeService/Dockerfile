#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["LikeService/LikeService.csproj", "LikeService/"]
COPY ["InstagramSD/Common/Common.csproj", "Common/"]
COPY ["InstagramSD/Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "LikeService/LikeService.csproj"
COPY . .
WORKDIR "/src/LikeService"
RUN dotnet build "LikeService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LikeService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LikeService.dll"]
