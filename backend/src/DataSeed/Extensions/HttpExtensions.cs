using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace Backend.DataSeed.Extensions;

public static class HttpExtensions
{
    public static StringContent ToStringContent(this object obj)
    {
        var options = new JsonSerializerOptions();
        return new StringContent(
            JsonSerializer.Serialize(obj, options),
            Encoding.UTF8,
            MediaTypeNames.Application.Json
        );
    }

    public static HttpClient SetJwtToken(this HttpClient client, string token)
    {
        client.ClearToken();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        return client;
    }

    public static HttpClient ClearToken(this HttpClient client)
    {
        client.DefaultRequestHeaders.Remove("Authorization");
        return client;
    }
}
