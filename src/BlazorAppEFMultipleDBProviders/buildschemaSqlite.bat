
:: on solution folder:::


::rmdir /S /Q Migrations

echo "Start: Sqlite Migration"
dotnet ef migrations add Initial01 -c ApplicationDbContext  -o ./Migrations/Initial01 --startup-project ./BlazorAppEFMultipleDBProviders --project ./AppMigrations/AppMigrations.Sqlite -- --DatabaseProvider Sqlite

dotnet ef migrations script -c ApplicationDbContext  -o ./AppMigrations/AppMigrations.Sqlite/Migrations/Initial01.sql --startup-project ./BlazorAppEFMultipleDBProviders --project ./AppMigrations/AppMigrations.Sqlite -- --DatabaseProvider Sqlite

:: MUST BE: appsettings.json  ==> "DatabaseProvider": "Sqlite" 
dotnet run --project ./BlazorAppEFMultipleDBProviders/BlazorAppEFMultipleDBProviders.csproj /seed

pause