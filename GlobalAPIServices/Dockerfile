#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GlobalAPIServices/GlobalAPIServices.csproj", "GlobalAPIServices/"]
COPY ["GlobalAPIServices.Application/GlobalAPIServices.Application.csproj", "GlobalAPIServices.Application/"]
COPY ["GlobalAPIServices.Infrastracture.Repository/GlobalAPIServices.Infrastracture.Repository.csproj", "GlobalAPIServices.Infrastracture.Repository/"]
COPY ["GlobalAPIServices.Domain.Model/GlobalAPIServices.Domain.Model.csproj", "GlobalAPIServices.Domain.Model/"]
COPY ["GlobalAPIServices.LoggerService/GlobalAPIServices.LoggerService.csproj", "GlobalAPIServices.LoggerService/"]
RUN dotnet restore "GlobalAPIServices/GlobalAPIServices.csproj"
COPY . .
WORKDIR "/src/GlobalAPIServices"
RUN dotnet build "GlobalAPIServices.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GlobalAPIServices.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GlobalAPIServices.dll"]