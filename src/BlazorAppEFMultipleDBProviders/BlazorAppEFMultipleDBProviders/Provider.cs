namespace BlazorAppEFMultipleDBProviders;

public record Provider(string Name, string Assembly)
{
    public static readonly Provider Sqlite = new(nameof(Sqlite), typeof(AppMigrations.Sqlite.Marker).Assembly.GetName().Name!);
    public static readonly Provider SqlServer = new(nameof(SqlServer), typeof(AppMigrations.SqlServer.Marker).Assembly.GetName().Name!);
    //public static readonly Provider Postgres = new(nameof(Postgres), typeof(Postgres.Marker).Assembly.GetName().Name!);
}