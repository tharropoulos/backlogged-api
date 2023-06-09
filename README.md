# Backlogged API

This is the API for Backlogged, written in ASP.NET Core 7, utilizing Swagger Pages.
You can learn more about Backlogged at https://github.com/tharropoulos/backlogged-swe1.

## Finding it online

You can find the Swagger page on https://backlogged-api-production.up.railway.app/swagger/index.html.

## Running it yourself

In order to run this yourself, you need to have the latest [.NET SDK and .NET Runtime](https://learn.microsoft.com/en-us/dotnet/core/install/) installed, along with [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/).
After installing all dependencies, you need to create an `appsettings.json` file to specify your database connection string and JWT secret, after which you can run

```bash
dotnet ef core database update
```

in order to make all the migrations for your database and finally run

```bash
dotnet run
```

to run the application. The default localhost port is 8081 and you can navigate at https://localhost:8081/swagger to use the Swagger page. For the endpoints to work you need to first register using the `POST: /user` endpoint and then login through the `POST: /user/login` method to get your JWT. After acquiring your API key you press on the 'Authorize' button on top of the Swagger page and in the 'Value' from provide your key in this format:

```bash
Bearer <key>
```

If for whatever reason you type Bearer before your key, you can't be authenticated.
