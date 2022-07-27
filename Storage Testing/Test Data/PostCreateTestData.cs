using BlogAPI.Application.ApiModels;
using System.Collections;

namespace BlogAPI.Application.TestData;

public class PostCreateTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        //Name Minimuim Length 4
        yield return new object[] { new CreateBlog("123") };

        //Name Maximum Length 24
        yield return new object[] { new CreateBlog(Faker.Lorem.Sentence(25)) };

        //Summary Maximum Length 300
        yield return new object[] { new CreateBlog("Good Name", Faker.Lorem.Sentence(301)) };

        //Summary Minimum Length 1
        yield return new object[] { new CreateBlog("Good Name", "") };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
