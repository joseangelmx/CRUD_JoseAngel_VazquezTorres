#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Journeys.WebApi/Journeys.WebApi.csproj", "Journeys.WebApi/"]
COPY ["Journeys.Shared/Journeys.Shared.csproj", "Journeys.Shared/"]
COPY ["Journeys.ApplicationServices/Journeys.ApplicationServices.csproj", "Journeys.ApplicationServices/"]
COPY ["Journeys.DataAccess/Journeys.DataAccess.csproj", "Journeys.DataAccess/"]
COPY ["Journeys.Core/Journeys.Core.csproj", "Journeys.Core/"]
RUN dotnet restore "Journeys.WebApi/Journeys.WebApi.csproj"
COPY . .
WORKDIR "/src/Journeys.WebApi"
RUN dotnet build "Journeys.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Journeys.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Journeys.WebApi.dll"]