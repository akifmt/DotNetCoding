using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using System.Globalization;

namespace BlazorAppRadzenGlobalizationLocalization.Shared
{
    public partial class CulturePicker
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        //protected string culture;
        //protected dynamic cultureDrowDownData = new[] {
        //    new { Text = "English (United States)", Value = "en-US" },
        //    new { Text = "English (United Kingdom)", Value = "en-GB" },
        //    new { Text = "Türkçe", Value = "tr-TR" }
        //};

        protected string language;

        protected dynamic languageDrowDownData = new[] {
            new { Text = "English", Value = "en" },
            new { Text = "Türkçe", Value = "tr" } };

        protected string currency;

        protected dynamic currencyDrowDownData = new[] {
            new { Text = $"USD ({new RegionInfo( "US" ).CurrencySymbol})", Value = "US" },
            new { Text = $"TRY ({new RegionInfo( "TR" ).CurrencySymbol})", Value = "TR" } };

        protected override void OnInitialized()
        {
            //culture = CultureInfo.CurrentCulture.Name;

            language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            currency = CultureInfo.CurrentCulture.Name.ReplaceFirst($"{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}-", "");
        }

        protected void ChangeCulture()
        {
            var redirect = new Uri(NavigationManager.Uri).GetComponents(UriComponents.PathAndQuery | UriComponents.Fragment, UriFormat.UriEscaped);

            string culture = $"{language}-{currency}";

            var query = $"?culture={Uri.EscapeDataString(culture)}&redirectUri={redirect}";

            NavigationManager.NavigateTo("Culture/SetCulture" + query, forceLoad: true);
        }
    }
}