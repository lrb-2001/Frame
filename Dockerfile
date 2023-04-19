# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ADD railway.sh ./railway.sh

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Module/01/FrameMiscellaneous/FrameMiscellaneous.csproj", "Module/01/FrameMiscellaneous/"]
RUN dotnet restore "Module/01/FrameMiscellaneous/FrameMiscellaneous.csproj"
COPY . .
WORKDIR "/src/Module/01/FrameMiscellaneous"
RUN dotnet build "FrameMiscellaneous.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FrameMiscellaneous.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "FrameMiscellaneous.dll"]
ENTRYPOINT ["/bin/sh", "./railway.sh"]