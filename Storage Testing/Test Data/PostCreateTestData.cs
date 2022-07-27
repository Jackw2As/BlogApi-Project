using BlogAPI.Application.ApiModels;
using System.Collections;

namespace BlogAPI.Application.TestData;

public class PostCreateTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        //Title Minimuim Length 3
        yield return new object[] { new CreatePost("12", "", (string)null, null) };

        //Title Max Length 100
        yield return new object[] { new CreatePost(Faker.Lorem.Sentence(101), "", (string)null, null) };

        //Summary Maximum Length 255
        yield return new object[] { new CreateBlog("Good Title", Faker.Lorem.Sentence(256)) };

        //Summary Minimum Length 1
        yield return new object[] { new CreateBlog("Good Title", "") };

        //Content can't be null
        yield return new object[] { new CreateBlog("Good Title", null) };

        //Content Max Length 5000
        yield return new object[] { new CreateBlog("Good Title", Faker.Lorem.Sentence(5001)) };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
