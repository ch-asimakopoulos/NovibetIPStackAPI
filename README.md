This is a .NET Core 3.1 Web API that provides Geolocation information that derives from an IP address using IPStack's API.

This project is setup to run on IISExpress and Kestrel and exposes Swagger's UI and OpenAPI specification. The Web API usage can be seen by running the Web API in the swagger UI page. You can also test calls there. 

To build this project you need to add an appsettings.json file that provides the configuration for your SQL Server connection string, your API key for the IPStack external API that is used to get the IP address related information and some configurations regarding in-memory cache, logging and batch update request processing. [sample appsettings.json file](https://github.com/ch-asimakopoulos/NovibetIPStackAPI/blob/main/src/NovibetIPStackAPI.WebApi/sampleSettings.json) 

Check [Initial Migration's documentation](https://github.com/ch-asimakopoulos/NovibetIPStackAPI/blob/main/initialMigration.md) on information on how to run your migrations.

For any more questions regarding this you can contact me at ch.asimakopoulos@gmail.com

