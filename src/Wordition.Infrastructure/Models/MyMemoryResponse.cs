using System.Text.Json.Serialization;

namespace Wordition.Infrastructure.Models;

public class MyMemoryResponse
{
    [JsonPropertyName("responseData")]
    public required MyMemoryResponseData ResponseData {get; set;}
}