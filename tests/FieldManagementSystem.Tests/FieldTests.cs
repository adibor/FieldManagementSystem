using System.Net;
using System.Net.Http.Json;
using Xunit;

public class FieldTests : IClassFixture<TestAppFactory>
{
    private readonly HttpClient _client;

    public FieldTests(TestAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task User_Can_Create_And_Get_Own_Field()
    {
        var token = await TestAuthHelper.LoginAsync(_client, "fielduser@test.com");
        TestAuthHelper.SetBearer(_client, token);

        // Create Field
        var createResponse = await _client.PostAsJsonAsync("/api/Fields", new
        {
            name = "Test Field",
            areaHectares = 10
        });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        // Get My Fields
        var getResponse = await _client.GetAsync("/api/Fields");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var body = await getResponse.Content.ReadAsStringAsync();
        Assert.Contains("Test Field", body);
    }
}
