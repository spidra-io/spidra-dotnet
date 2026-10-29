using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spidra.Types.Templates;

public class TemplateSummary
{
    [JsonPropertyName("slug")]
    public required string Slug { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("site")]
    public string? Site { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }
}

public class TemplateInput
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("required")]
    public bool Required { get; set; }

    [JsonPropertyName("default")]
    public JsonElement? Default { get; set; }

    [JsonPropertyName("options")]
    public List<JsonElement>? Options { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}

public class TemplateColumn
{
    [JsonPropertyName("key")]
    public required string Key { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}

public class TemplateSite
{
    [JsonPropertyName("slug")]
    public required string Slug { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }
}

public class TemplateDetail
{
    [JsonPropertyName("slug")]
    public required string Slug { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("category")]
    public string? Category { get; set; }

    [JsonPropertyName("site")]
    public string? Site { get; set; }

    [JsonPropertyName("inputs")]
    public List<TemplateInput> Inputs { get; set; } = [];

    [JsonPropertyName("columns")]
    public List<TemplateColumn> Columns { get; set; } = [];

    [JsonPropertyName("baseCreditCost")]
    public double BaseCreditCost { get; set; }

    [JsonPropertyName("creditCostFormula")]
    public string? CreditCostFormula { get; set; }
}

public class TemplateEstimate
{
    [JsonPropertyName("estimatedCredits")]
    public double EstimatedCredits { get; set; }

    [JsonPropertyName("breakdown")]
    public Dictionary<string, object?>? Breakdown { get; set; }
}

public class TemplateRunProgress
{
    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("completed")]
    public int Completed { get; set; }

    [JsonPropertyName("failed")]
    public int Failed { get; set; }

    [JsonPropertyName("percent")]
    public double Percent { get; set; }
}

public class TemplateRunResponse
{
    [JsonPropertyName("runId")]
    public required string RunId { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("inputs")]
    public Dictionary<string, object?>? Inputs { get; set; }

    [JsonPropertyName("result")]
    public List<Dictionary<string, object?>> Result { get; set; } = [];

    [JsonPropertyName("progress")]
    public TemplateRunProgress? Progress { get; set; }

    [JsonPropertyName("creditsUsed")]
    public double CreditsUsed { get; set; }

    [JsonPropertyName("totalRows")]
    public int TotalRows { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("createdAt")]
    public string? CreatedAt { get; set; }

    [JsonPropertyName("completedAt")]
    public string? CompletedAt { get; set; }
}

public class Airport
{
    [JsonPropertyName("iata")]
    public required string Iata { get; set; }

    [JsonPropertyName("icao")]
    public string? Icao { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }
}

public class ListTemplatesResponse
{
    [JsonPropertyName("templates")]
    public List<TemplateSummary> Templates { get; set; } = [];

    [JsonPropertyName("categories")]
    public List<string> Categories { get; set; } = [];

    [JsonPropertyName("sites")]
    public List<TemplateSite> Sites { get; set; } = [];

    [JsonPropertyName("total")]
    public int Total { get; set; }
}

public class ListTemplateRunsResponse
{
    [JsonPropertyName("runs")]
    public List<TemplateRunResponse> Runs { get; set; } = [];

    [JsonPropertyName("total")]
    public int Total { get; set; }
}

