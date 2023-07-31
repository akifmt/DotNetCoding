
:: on solution folder:::


::rmdir /S /Q Migrations

echo "Start: SqlServer Migration"
dotnet ef migrations add Initial01 -c ApplicationDbContext  -o ./Migrations/Initial01 --startup-project ./BlazorAppEFMultipleDBProviders --project ./AppMigrations/AppMigrations.SqlServer -- --DatabaseProvider SqlServer

dotnet ef migrations script -c ApplicationDbContext  -o ./AppMigrations/AppMigrations.SqlServer/Migrations/Initial01.sql --startup-project ./BlazorAppEFMultipleDBProviders --project ./AppMigrations/AppMigrations.SqlServer -- --DatabaseProvider SqlServer

:: MUST BE: appsettings.json  ==> "DatabaseProvider": "SqlServer" 
dotnet run --project ./BlazorAppEFMultipleDBProviders/BlazorAppEFMultipleDBProviders.csproj /seed

pause