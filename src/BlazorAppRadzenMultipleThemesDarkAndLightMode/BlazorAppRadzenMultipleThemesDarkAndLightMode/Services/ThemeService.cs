using System.Web;
using Microsoft.AspNetCore.Components;

namespace BlazorAppRadzenMultipleThemesDarkAndLightMode;

public class ThemeService
{
    public class Theme
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
    public static readonly Theme[] Themes = new[]
    {
        new Theme {
            Text = "Material",
            Value = "material"
        },
        new Theme {
            Text = "Standard",
            Value = "standard"
        },
        new Theme {
            Text = "Default",
            Value = "default"
        },
        new Theme {
            Text = "Humanistic",
            Value = "humanistic"
        },
        new Theme {
            Text = "Software",
            Value = "software"
        },
        new Theme {
            Text = "Dark",
            Value="dark"
        }
    };

    public const string DefaultTheme = "standard";
    public const string QueryParameter = "theme";

    public string CurrentTheme { get; set; } = DefaultTheme;

    public void Initialize(NavigationManager navigationManager)
    {
        var uri = new Uri(navigationManager.ToAbsoluteUri(navigationManager.Uri).ToString());
        var query = HttpUtility.ParseQueryString(uri.Query);
        var value = query.Get(QueryParameter);

        if (Themes.Any(theme => theme.Value == value))
        {
            CurrentTheme = value;
        }
    }

    public void Change(NavigationManager navigationManager, string theme)
    {
        var url = navigationManager.GetUriWithQueryParameters(navigationManager.Uri,
            new Dictionary<string, object>() { { QueryParameter, theme } });

        navigationManager.NavigateTo(url, true);
    }
}