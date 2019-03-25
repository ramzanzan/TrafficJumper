using System.Collections.Generic;

public interface IBlockModifier
{
    void Modify(LinkedList<IDescriptorWithID> list);
}
