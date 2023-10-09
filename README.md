# SolarWatch

## Overview
SolarWatch is a simple application designed to deliver precise sunrise and sunset information for a specified city. You can use the Swagger UI to test the application.

## Functionality
SolarWatch offers the following features:
- **User Registration Handling**: Manage user registration and authentication.
- **Retrieve Sunset & Sunrise for a Specific City**: Obtain accurate sunset and sunrise times for a chosen city.
- **Save & Edit Information for the Desired City**: Store and modify information about the city of interest.

## Installation
To set up the SolarWatch app, follow these steps:

1. Clone the repository:
```
$ git clone git@github.com:mayu0126/solarwatch.git
```

2. Navigate to the project directory:
```
$ cd solarwatch/SolarWatch
```

3. Start a Docker container with SQL Server:
```
$ docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=[yourStrongPassword]" -p 1433:1433 -d mcr.microsoft.com/mssql/server
```

4. Install the Entity Framework Core tools globally:
```
$ dotnet tool install --global dotnet-ef
```

5. Add the Entity Framework Core Design package to your project:
```
$ dotnet add package Microsoft.EntityFrameworkCore.Design
```

6. Create the initial database migration:
```
$ dotnet ef migrations add InitialCreate
```

7. Update the database schema:
```
$ dotnet ef database update
```

### Additional steps:
- Register on the https://openweathermap.org/api for your own api key, and change the "apiKey" variable in the GeocodingApi.cs class to yours, or set it as a user secret.
- Add your own database connection string in the SolarWatchContext.cs class to connect to the dockerized database, or set it as a user secret.

These steps will help you set up the SolarWatch and its database. Enjoy accurate sunrise and sunset data for your favorite cities!
