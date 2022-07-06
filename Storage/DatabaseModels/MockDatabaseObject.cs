namespace BlogAPI.Storage.DatabaseModels;

public class MockDatabaseObject : DataObject
{
    public bool ManipulateMe { get; set; }

    public MockDatabaseObject()
    {
        ID = Guid.NewGuid();

        ManipulateMe = true;
    }
}
