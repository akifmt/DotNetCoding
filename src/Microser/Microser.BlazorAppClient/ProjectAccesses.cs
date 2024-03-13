namespace Microser.BlazorAppClient;

public static class ProjectRoles
{
    public static ProjectRole UserRole => new ProjectRole { RoleName = "User" };
    public static ProjectRole AdminRole => new ProjectRole { RoleName = "Admin" };
    public static ProjectRole IdentityAdmin => new ProjectRole { RoleName = "IdentityAdmin" };

    public static ProjectRole[] Roles => new ProjectRole[] { UserRole, AdminRole, IdentityAdmin };
}

public static class ProjectPolicies
{
    public static ProjectPolicy UserRolePolicy => new ProjectPolicy
    {
        Name = nameof(UserRolePolicy),
        RequiredRoles = new[] { ProjectRoles.UserRole }
    };

    public static ProjectPolicy IdentityAdminRolePolicy => new ProjectPolicy
    {
        Name = nameof(IdentityAdminRolePolicy),
        RequiredRoles = new[] { ProjectRoles.IdentityAdmin }
    };

    public static ProjectPolicy AdminRolePolicy => new ProjectPolicy
    {
        Name = nameof(AdminRolePolicy),
        RequiredRoles = new[] { ProjectRoles.AdminRole }
    };

    public static ProjectPolicy WeatherForecastCreatePolicy => new ProjectPolicy
    {
        Name = nameof(WeatherForecastCreatePolicy),
        RequiredClaims = new[] { ProjectClaims.WeatherForecastCreate }
    };

    public static ProjectPolicy WeatherForecastReadPolicy => new ProjectPolicy
    {
        Name = nameof(WeatherForecastReadPolicy),
        RequiredClaims = new[] { ProjectClaims.WeatherForecastRead }
    };

    public static ProjectPolicy WeatherForecastUpdatePolicy => new ProjectPolicy
    {
        Name = nameof(WeatherForecastUpdatePolicy),
        RequiredClaims = new[] { ProjectClaims.WeatherForecastUpdate }
    };

    public static ProjectPolicy WeatherForecastDeletePolicy => new ProjectPolicy
    {
        Name = nameof(WeatherForecastDeletePolicy),
        RequiredClaims = new[] { ProjectClaims.WeatherForecastDelete }
    };
}

public class ProjectPolicy
{
    public string Name { get; set; }
    public ProjectClaim[] RequiredClaims { get; set; }
    public ProjectRole[] RequiredRoles { get; set; }
}

public static class ProjectClaims
{
    public static ProjectClaim WeatherForecastCreate => new ProjectClaim { Type = "WeatherForecast", Value = "Create" };
    public static ProjectClaim WeatherForecastRead => new ProjectClaim { Type = "WeatherForecast", Value = "Read" };
    public static ProjectClaim WeatherForecastUpdate => new ProjectClaim { Type = "WeatherForecast", Value = "Update" };
    public static ProjectClaim WeatherForecastDelete => new ProjectClaim { Type = "WeatherForecast", Value = "Delete" };

    public static List<ProjectClaim> GetMovieClaims => new List<ProjectClaim> { WeatherForecastCreate, WeatherForecastRead, WeatherForecastUpdate, WeatherForecastDelete };
}

public class ProjectClaim
{
    public string Type { get; set; }
    public string Value { get; set; }
}

public class ProjectRole
{
    public string RoleName { get; set; }
}