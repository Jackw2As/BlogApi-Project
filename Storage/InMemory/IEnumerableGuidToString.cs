using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BlogAPI.Storage.InMemory;

public class IEnumerableGuidToString : ValueConverter<List<string>, string>
{
    public IEnumerableGuidToString() : base (

        list => String.Join(",,,", list.Select(p => p.ToString().ToArray())),
        Strings => Strings.Split(",,,", StringSplitOptions.None).ToList()
    )
    {

    }
}


