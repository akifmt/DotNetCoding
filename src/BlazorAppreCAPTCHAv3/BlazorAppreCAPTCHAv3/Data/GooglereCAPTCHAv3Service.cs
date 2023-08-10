using System.Text.Json;

namespace BlazorAppreCAPTCHAv3.Data;

public class GooglereCAPTCHAv3Service
{
    public virtual async Task<GooglereCAPTCHAv3Response?> Verify(string token)
    {
        GooglereCAPTCHAv3Response? reCaptchaResponse;
        using (var httpClient = new HttpClient())
        {
            var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("secret", GooglereCAPTCHAv3Settings.SecretKey),
                new KeyValuePair<string, string>("response", token)
            });
            try
            {
                var response = await httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify", content);
                var jsonString = await response.Content.ReadAsStringAsync();
                reCaptchaResponse = JsonSerializer.Deserialize<GooglereCAPTCHAv3Response>(jsonString);
            }
            catch (Exception)
            {
                throw;
            }

            return reCaptchaResponse;
        }
    }
}