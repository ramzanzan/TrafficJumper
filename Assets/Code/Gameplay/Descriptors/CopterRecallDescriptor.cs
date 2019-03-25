public class CopterRecallDescriptor : IDescriptorWithID
{
    private const string Tag = "Copter";
    
    public int GetID()
    {
        return -1;
    }

    public void SetID(int id)
    {}

    public string GetTag()
    {
        return Tag;
    }
}
