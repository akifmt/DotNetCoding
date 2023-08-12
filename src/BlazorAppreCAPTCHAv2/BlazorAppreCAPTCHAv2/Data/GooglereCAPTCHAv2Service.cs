namespace BlazorAppreCAPTCHAv2.Data;

public class GooglereCAPTCHAv2Service
{
    public async Task<(bool Success, string[]? ErrorCodes)> Post(string reCAPTCHAResponse)
    {
        var url = "https://www.google.com/recaptcha/api/siteverify";
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"secret", GooglereCAPTCHAv2Settings.SecretKey},
                {"response", reCAPTCHAResponse}
            });

        GooglereCAPTCHAv2Response? verificationResponse;
        using (HttpClient httpClient = new HttpClient())
        {
            try
            {
                var response = await httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                verificationResponse = await response.Content.ReadFromJsonAsync<GooglereCAPTCHAv2Response>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        if (verificationResponse is null || !verificationResponse.Success)
            return (false, verificationResponse?.ErrorCodes.Select(err => err.Replace('-', ' ')).ToArray());

        return (Success: true, ErrorCodes: null);
    }
}