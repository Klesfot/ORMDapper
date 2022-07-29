using System.Text.Json;

namespace ORMDapper.Tests.Helpers;

public static class AnonymousTypeExtensions
{
    private readonly static JsonSerializerOptions options = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    public static bool JsonMatches(this object o, object that)
    {
        return JsonSerializer.Serialize(o, options) == JsonSerializer.Serialize(that, options);
    }
}