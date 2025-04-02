FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY CourseProject.sln . 
COPY CourseProject/*.csproj CourseProject/

WORKDIR /app/CourseProject
RUN dotnet restore

COPY . .

RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .

EXPOSE 80
ENTRYPOINT ["dotnet", "CourseProject.dll"]
