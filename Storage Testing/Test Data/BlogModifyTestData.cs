using BlogAPI.Application.ApiModels;
using System.Collections;

namespace BlogAPI.Application.TestData;

public class BlogModifyTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        //Content can't be null
        yield return new object[] { new ModifyComment((string)null) };

        //Content max length is 300
        yield return new object[] { new ModifyComment(Faker.Lorem.Sentence(301)) };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
