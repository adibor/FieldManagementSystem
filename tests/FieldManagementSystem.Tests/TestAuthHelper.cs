using System.Net.Http.Headers;
using System.Net.Http.Json;

public static class TestAuthHelper
{
    public static async Task<string> LoginAsync(HttpClient client, string email)
    {
        var response = await client.PostAsJsonAsync("/api/Auth/login", new
        {
            email
        });

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<AuthResponse>();
        return json!.Token;
    }

    public static void SetBearer(HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    private class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}
