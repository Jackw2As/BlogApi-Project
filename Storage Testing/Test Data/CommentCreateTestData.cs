using BlogAPI.Application.ApiModels;
using System.Collections;

namespace BlogAPI.Application.TestData;

public class CommentCreateTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        //Username can't be null
        yield return new object[] { new CreateComment(null, string.Empty, string.Empty) };

        //Username max length 20
        yield return new object[] { new CreateComment(Faker.Lorem.Sentence(21), string.Empty, string.Empty) };

        //Username should not contain whitespace.
        yield return new object[] { new CreateComment("Invalid User Name", null, string.Empty) };

        //Content can't be null
        yield return new object[] { new CreateComment("ValidUserName", null, string.Empty) };

        //Content max length is 300
        yield return new object[] { new CreateComment("ValidUserName", Faker.Lorem.Sentence(301), string.Empty) };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
