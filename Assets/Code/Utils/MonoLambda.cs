using System;
using UnityEngine;

public class MonoLambda : MonoBehaviour
{
    public object Parameters;
    public Action<object> Function;

    public void Call()
    {
        Function?.Invoke(Parameters);
    }
}
