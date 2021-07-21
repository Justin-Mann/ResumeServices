#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["InstitutionAPI/InstitutionAPI.csproj", "InstitutionAPI/"]
COPY ["ResumeCore/ResumeCore.csproj", "ResumeCore/"]
COPY ["ResumeInfastructure/ResumeInfastructure.csproj", "ResumeInfastructure/"]
RUN dotnet restore "InstitutionAPI/InstitutionAPI.csproj"
COPY . .
WORKDIR "/src/InstitutionAPI"
RUN dotnet build "InstitutionAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InstitutionAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InstitutionAPI.dll"]