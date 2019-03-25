using System;
using System.Collections.Generic;
using UnityEngine;

public interface IMiniGenerator
{
    bool HasMore { get; }
    void GenerateBlock(LinkedList<IDescriptorWithID> list);
    bool WasBlockModifable { get; }
    Func<IDictionary<int, GameObject>, LinkedList<IDescriptorWithID>, float, bool> ReadinessTestHandler { get; }
    void Reset();
}
