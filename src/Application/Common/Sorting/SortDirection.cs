using System.Text.Json.Serialization;

namespace BackendAuthTemplate.Application.Common.Sorting
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortDirection
    {
        Ascending,
        Descending
    }
}
