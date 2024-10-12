#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["PostProcessor/PostProcessor.csproj", "PostProcessor/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "PostProcessor/PostProcessor.csproj"
COPY . .
WORKDIR "/src/PostProcessor"
RUN dotnet build "PostProcessor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PostProcessor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PostProcessor.dll"]
