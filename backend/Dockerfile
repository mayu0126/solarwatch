# Use a suitable .NET SDK base image -> pay attention to SDK!!!
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy the .csproj file and restore the dependencies. Here you must specify the folder before the * in which the csproj file is located
COPY /SolarWatch *.csproj ./
RUN dotnet restore

# Copy all source code
COPY . ./
RUN dotnet publish -c Release -o out

# Use a smaller base image to run -> pay attention to SDK!!!
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .

# set the port
ENV ASPNETCORE_URLS=http://+:80

# Application start command
ENTRYPOINT ["dotnet", "SolarWatch.dll"]

