using BlogAPI.Application.ApiModels;
using System.Collections;

namespace BlogAPI.Application.TestData;

public class BlogModifyTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        //Name too short (min 4)
        yield return new object[] { new ModifyBlog("123") };

        //Name too long (max 24)
        yield return new object[] { new ModifyBlog(Faker.Lorem.Sentence(25)) };

        //Summary Minimum Length 1
        yield return new object[] { new ModifyBlog("Good Name", "") };

        //Summary too long(max length 300)
        yield return new object[] { new ModifyBlog("Good Name", Faker.Lorem.Sentence(301)) };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
