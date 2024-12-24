using System.Text.Json;

namespace Core.Application.Helpers;

public class SerializerHelper
{
    /// <summary>
    /// JSON Serialization
    /// </summary>
    public static string JsonSerialize<T>(T t)
    {
        return JsonSerializer.Serialize(t);
    }

    /// <summary>
    /// JSON Deserialization
    /// </summary>
    public static T? JsonDeserialize<T>(string jsonString)
    {
        return JsonSerializer.Deserialize<T>(jsonString);
    }
}