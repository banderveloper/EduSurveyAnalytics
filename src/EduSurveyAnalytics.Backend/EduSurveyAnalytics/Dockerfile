FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ARG POSTGRES_DB_USERNAME
ARG POSTGRES_DB_PASSWORD

ENV Database__Username=$POSTGRES_DB_USERNAME
ENV Database__Password=$POSTGRES_DB_PASSWORD

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EduSurveyAnalytics.Domain/EduSurveyAnalytics.Domain.csproj", "EduSurveyAnalytics.Domain/EduSurveyAnalytics.Domain.csproj"]
COPY ["EduSurveyAnalytics.Application/EduSurveyAnalytics.Application.csproj", "EduSurveyAnalytics.Application/EduSurveyAnalytics.Application.csproj"]
COPY ["EduSurveyAnalytics.Persistence/EduSurveyAnalytics.Persistence.csproj", "EduSurveyAnalytics.Persistence/EduSurveyAnalytics.Persistence.csproj"]
COPY ["EduSurveyAnalytics.WebApi/EduSurveyAnalytics.WebApi.csproj", "EduSurveyAnalytics.WebApi/EduSurveyAnalytics.WebApi.csproj"]

COPY . .
WORKDIR "/src/EduSurveyAnalytics.WebApi"
RUN dotnet build "EduSurveyAnalytics.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EduSurveyAnalytics.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EduSurveyAnalytics.WebApi.dll"]