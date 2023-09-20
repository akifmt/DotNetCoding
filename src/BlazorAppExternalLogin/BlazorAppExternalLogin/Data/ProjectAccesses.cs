namespace BlazorAppExternalLogin.Data;

public static class ProjectRoles
{
    public static ProjectRole UserRole => new ProjectRole { RoleName = "User", NormalizedName = "USER", Description = "User Role" };
    public static ProjectRole AdminRole => new ProjectRole { RoleName = "Admin", NormalizedName = "ADMIN", Description = "Admin Role" };

    public static ProjectRole[] Roles => new ProjectRole[] { UserRole, AdminRole };
}

public static class ProjectPolicies
{
    public static ProjectPolicy BlogPostCreateClaimPolicy => new ProjectPolicy
    {
        Name = nameof(BlogPostCreateClaimPolicy),
        RequiredClaims = new[] { ProjectClaims.BlogPostCreate }
    };

    public static ProjectPolicy BlogPostReadClaimPolicy => new ProjectPolicy
    {
        Name = nameof(BlogPostReadClaimPolicy),
        RequiredClaims = new[] { ProjectClaims.BlogPostRead }
    };

    public static ProjectPolicy BlogPostUpdateClaimPolicy => new ProjectPolicy
    {
        Name = nameof(BlogPostUpdateClaimPolicy),
        RequiredClaims = new[] { ProjectClaims.BlogPostUpdate }
    };

    public static ProjectPolicy BlogPostDeleteClaimPolicy => new ProjectPolicy
    {
        Name = nameof(BlogPostDeleteClaimPolicy),
        RequiredClaims = new[] { ProjectClaims.BlogPostDelete }
    };
}

public static class ProjectClaims
{
    public static ProjectClaim BlogPostCreate => new ProjectClaim { Type = "BlogPost", Value = "Create" };
    public static ProjectClaim BlogPostRead => new ProjectClaim { Type = "BlogPost", Value = "Read" };
    public static ProjectClaim BlogPostUpdate => new ProjectClaim { Type = "BlogPost", Value = "Update" };
    public static ProjectClaim BlogPostDelete => new ProjectClaim { Type = "BlogPost", Value = "Delete" };
    public static List<ProjectClaim> GetBlogPostClaims => new List<ProjectClaim> { BlogPostCreate, BlogPostRead, BlogPostUpdate, BlogPostDelete };

    public static List<List<ProjectClaim>> GetAllClaims = new List<List<ProjectClaim>> { GetBlogPostClaims };
}

public class ProjectPolicy
{
    public string Name { get; set; }
    public ProjectClaim[] RequiredClaims { get; set; }
    public ProjectRole[] RequiredRoles { get; set; }
}

public class ProjectClaim
{
    public string Type { get; set; }
    public string Value { get; set; }
}

public class ProjectRole
{
    public string RoleName { get; set; }
    public string NormalizedName { get; set; }
    public string Description { get; set; }
}