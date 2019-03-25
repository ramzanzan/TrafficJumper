using JetBrains.Annotations;
using UnityEngine;

public interface IBuilder
{
    [CanBeNull]
    GameObject Build(IDescriptorWithID desc);
    
    void Disassemble(GameObject go);

    void RecallAll();
    
    void SetStyle(StyleAssets style);
}
