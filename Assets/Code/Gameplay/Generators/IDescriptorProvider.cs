using System;
using UnityEngine;

public interface IDescriptorProvider
{
    void Reset(CarDescriptor enter);
    bool IsReadyForMore();
    bool HasNext();
    IDescriptorWithID Next();
    bool HasMore();
    void SetIDGenerator(IDGenerator gen);
}
