using Microser.BlazorAppClient.Extensions;
using Microser.Core.Models;

namespace Microser.BlazorAppClient.Services;

public class WeatherForecastApiService : IWeatherForecastApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WeatherForecastApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public async Task<WeatherForecast[]?> GetAllAsync()
    {
        // get from gateway
        var httpClient = _httpClientFactory.CreateClient("APIGateway");
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            "/WeatherForecast");
        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        //// directly from API
        //var httpClient = _httpClientFactory.CreateClient("WeatherForecastAPIClient");
        //var request = new HttpRequestMessage(
        //    HttpMethod.Get,
        //    "/api/WeatherForecast");
        //var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
        //    .ConfigureAwait(false);
        //response.EnsureSuccessStatusCode();

        //var content = await response.Content.ReadAsStringAsync();
        //var weatherForecastList = JsonSerializer.Deserialize<List<WeatherForecast>>(content);

        var weatherForecastList = await response.ReadContentAs<WeatherForecast[]>();
        return weatherForecastList;

        #region Another Way

        // added for testing

        //var apiClientCredentials = new ClientCredentialsTokenRequest
        //{
        //    Address = "https://localhost:5005/connect/token",

        //    ClientId = "movieClient",
        //    ClientSecret = "D04449B9D7BB46C7AF8B5951076115F3",

        //    Scope = "movieAPI"
        //};

        //var client = new HttpClient();

        //var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5005");
        //if (disco.IsError)
        //{
        //    return null; // throw 500 error
        //}

        //var tokenResponse = await client.RequestClientCredentialsTokenAsync(apiClientCredentials);
        //if (tokenResponse.IsError)
        //{
        //    return null;
        //}

        //var apiClient = new HttpClient();

        //apiClient.SetBearerToken(tokenResponse.AccessToken);

        //var response = await apiClient.GetAsync("https://localhost:6001/api/movies");
        //response.EnsureSuccessStatusCode();
        //var content = await response.Content.ReadAsStringAsync();

        //List<Movie> movieList = JsonSerializer.Deserialize<List<Movie>>(content);
        //return movieList;

        #endregion Another Way
    }

    public async Task<WeatherForecast?> GetByIdAsync(int id)
    {
        var httpClient = _httpClientFactory.CreateClient("APIGateway");
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            "/WeatherForecast/" + id);
        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var item = await response.ReadContentAs<WeatherForecast>();
        return item;
    }

    public async Task<WeatherForecast?> AddAsync(WeatherForecast weatherForecast)
    {
        var httpClient = _httpClientFactory.CreateClient("APIGateway");
        var request = new HttpRequestMessage(
            HttpMethod.Post, "/WeatherForecast")
        {
            Content = JsonContent.Create(weatherForecast)
        };

        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        weatherForecast = await response.ReadContentAs<WeatherForecast>();
        return weatherForecast;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        var httpClient = _httpClientFactory.CreateClient("APIGateway");
        var request = new HttpRequestMessage(
            HttpMethod.Delete, "/WeatherForecast/" + id.ToString());

        try
        {
            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
            .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> UpdateAsync(int id, WeatherForecast weatherForecast)
    {
        var httpClient = _httpClientFactory.CreateClient("APIGateway");
        var request = new HttpRequestMessage(
            HttpMethod.Put, "/WeatherForecast/" + weatherForecast.Id.ToString())
        {
            Content = JsonContent.Create(weatherForecast)
        };

        try
        {
            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
            .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }
}