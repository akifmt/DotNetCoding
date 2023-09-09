using System.Text.Json.Serialization;

namespace BlazorAppRadzenLoading.Models;

public class Organization
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("public_repos")]
    public int TotalPublicRepos { get; set; }

    [JsonPropertyName("followers")]
    public int TotalFollowers { get; set; }

    [JsonPropertyName("following")]
    public int TotalFollowings { get; set; }
}

public class Repo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("watchers")]
    public int WatchersCount { get; set; }

    [JsonPropertyName("updated_at")]
    public string UpdatedAt { get; set; } = string.Empty;
}

public class Issue
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("state")]
    public string State { get; set; } = string.Empty;

    [JsonPropertyName("updated_at")]
    public string UpdatedAt { get; set; } = string.Empty;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("labels")]
    public IEnumerable<IssueLabel> Labels { get; set; }
}

public class IssueGroup
{
    public string StateName { get; set; } = string.Empty;
    public string LabelName { get; set; } = string.Empty;
    public int Count { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedAtString => CreatedAt.ToString("yyyy/MM/dd");
}

public class IssueLabel
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;
}

public class IssueGroupbyLabel
{
    public string LabelName { get; set; } = string.Empty;
    public int Count { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedAtString => CreatedAt.ToString("yyyy/MM/dd");
}