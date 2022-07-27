using BlogAPI.Application.ApiModels;
using System.Collections;

namespace BlogAPI.Storage.InMemory;

public class PostModifyTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        //Title can't be null
        yield return new object[] { new ModifyPost(null, "some content") };

        //Title min 3
        yield return new object[] { new ModifyPost("12", "some content") };

        //Title max 100
        yield return new object[] { new ModifyPost(Faker.Lorem.Sentence(101), "some content") };

        //Summary max 255
        yield return new object[] { new ModifyPost("Great Title", "some content", Faker.Lorem.Sentence(256)) };

        //Content can't be null
        yield return new object[] { new ModifyPost("Great Title", null) };

        //Content max 5000
        yield return new object[] { new ModifyPost("Great Title", Faker.Lorem.Sentence(5000)) };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
