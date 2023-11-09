# SolarWatch

## Overview
SolarWatch is a simple application designed to deliver precise sunrise and sunset information for a specified city. Check out the deployed application on the link below:
🌟 https://solarwatch-mayu0126.onrender.com/ 🌟

![image](https://github.com/mayu0126/solarwatch/assets/117304817/ccded9ee-615e-4214-a9b4-26ad097794f4)

## Functionality
SolarWatch offers the following features:
- **User Registration Handling**: Manage user registration and authentication.
- **Retrieve Sunset & Sunrise for a Specific City**: Register and log in and obtain accurate sunset and sunrise times for a chosen city.

## Languages/Tools/Technologies
- C# (backend)
- JavaScript (frontend)
- HTML and CSS
- Node.js
- React.js
- ASP.NET Web API
- Entity Framework Core
- ASP.NET Core Identity
- PostgreSQL (for the deployed application) or Microsoft SQL Server (to run locally)
- Docker
- Sunset and sunrise times API
- OpenWeather's Geocoding API


## Installation
To set up the SolarWatch app, follow these steps:

1. Clone the repository:
```
$ git clone git@github.com:mayu0126/solarwatch.git
```

2. Navigate to the project directory:
```
$ cd solarwatch/backend/SolarWatch
```

3. Start a Docker container with SQL Server:
```
$ docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=[yourStrongPassword]" -p 1433:1433 -d mcr.microsoft.com/mssql/server
```

4. Connect to the database:
Use the Azure Data Studio or any other database tool to check the connection.
The username to login to the database is sa (for System Admin), and the password is what you've specified when running the docker command.

5. Add your own database connection string:
Add your own database connection string to the codebase where necessary in order to connect to the dockerized database, or set it as a user secret.
```
"Server=localhost,1433; Database=[databaseName]; User Id=sa; Password=[yourStrongPassword]; TrustServerCertificate=True;"
```
6. Add Entity Framework Core SQL Server provider:
```
$ dotnet add package Microsoft.EntityFrameworkCore.SqlServer -v 6.0.18
```

7. Install the Entity Framework Core tools globally:
```
$ dotnet tool install --global dotnet-ef
```

8. Add the Entity Framework Core Design package to your project:
```
$ dotnet add package Microsoft.EntityFrameworkCore.Design -v 6.0.18
```

9. Create the initial database migration:
```
$ dotnet ef migrations add InitialCreate --context UsersContext
```

10. Update the database schema:
```
$ dotnet ef database update --context UsersContext
```

11. Create migration for the SolarWatchContext as well and update it:
```
$ dotnet ef migrations add InitialCreate --context SolarWatchContext
$ dotnet ef database update --context SolarWatchContext
```

12. Start the SolarWatch backend:
```
$ cd backend/SolarWatch
$ dotnet run
```

13. Install frontend dependencies:
```
$ cd frontend
$ npm install
```

14. Start the frontend development server:
```
$ npm start
```

### Additional steps:
- Register on the https://openweathermap.org/api for your own api key, and change the "apiKey" variable in the GeocodingApi.cs class to yours, or set it as a user secret.

These steps will help you set up the SolarWatch and its database. Enjoy accurate sunrise and sunset data for your favorite cities!
