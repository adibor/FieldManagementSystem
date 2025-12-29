using System.Net;
using System.Net.Http.Json;
using Xunit;

public class AuthTests : IClassFixture<TestAppFactory>
{
    private readonly HttpClient _client;

    public AuthTests(TestAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_Returns_Jwt_Token()
    {
        var response = await _client.PostAsJsonAsync("/api/Auth/login", new
        {
            email = "user@test.com"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("token", body.ToLower());
    }
}
