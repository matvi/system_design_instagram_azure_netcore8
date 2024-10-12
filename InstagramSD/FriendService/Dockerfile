#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["FriendService/FriendService.csproj", "/FriendService/"]
COPY ["InstagramSD/Domain/Domain.csproj", "Domain/"]
COPY ["InstagramSD/Common/Common.csproj", "Common/"]
RUN dotnet restore "/FriendService/FriendService.csproj"
COPY . .
WORKDIR "/src/FriendService"
RUN dotnet build "FriendService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FriendService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FriendService.dll"]
