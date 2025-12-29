using System.Net;
using Xunit;

public class AuthorizationTests : IClassFixture<TestAppFactory>
{
    private readonly HttpClient _client;

    public AuthorizationTests(TestAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Regular_User_Cannot_Access_Admin_Endpoint()
    {
        var token = await TestAuthHelper.LoginAsync(_client, "user@test.com");
        TestAuthHelper.SetBearer(_client, token);

        var response = await _client.GetAsync("/api/Users");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
