using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace BlogAPI.Storage.InMemory;

public class EnumerableGuidToString : ValueConverter<List<string>, string>
{
    public EnumerableGuidToString() : base (

        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
        v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
    )
    {

    }
}


