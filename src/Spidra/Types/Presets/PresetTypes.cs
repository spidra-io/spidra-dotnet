using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spidra.Types.Presets;

public class Preset
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("tool")]
    public required string Tool { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("templateSlug")]
    public string? TemplateSlug { get; set; }

    [JsonPropertyName("config")]
    public Dictionary<string, object?>? Config { get; set; }

    [JsonPropertyName("createdAt")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public string? UpdatedAt { get; set; }
}

public class PresetParams
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("tool")]
    public required string Tool { get; set; }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; set; }

    [JsonPropertyName("templateSlug")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TemplateSlug { get; set; }

    [JsonPropertyName("config")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object?>? Config { get; set; }
}

public class RunPresetResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("jobId")]
    public string? JobId { get; set; }

    [JsonPropertyName("batchId")]
    public string? BatchId { get; set; }

    [JsonPropertyName("runId")]
    public string? RunId { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("result")]
    public JsonElement? Result { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }
}

public class ListPresetsResponse
{
    [JsonPropertyName("presets")]
    public List<Preset> Presets { get; set; } = [];

    [JsonPropertyName("total")]
    public int Total { get; set; }
}

