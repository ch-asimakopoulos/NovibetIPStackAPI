Both these commands need to be run on a console prompt opened in your repo's src folder.

dotnet ef migrations add initialMigration --project NovibetIPStackAPI.Infrastructure\ --startup-project NovibetIPStackAPI.WebApi\ --output-dir "YOUR_PATH_HERE\src\NovibetIPStackAPI.Infrastructure\Persistence\Migrations"

dotnet ef database update --project NovibetIPStackAPI.Infrastructure\ --startup-project NovibetIPStackAPI.WebApi\