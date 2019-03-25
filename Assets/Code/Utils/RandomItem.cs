using System;
using System.Collections;
using System.Collections.Generic;

public class RandomItem<T> : IEnumerator<T>
{
    private const float Delta = 0.0001f;
    private readonly Random _random = new Random();
    private readonly float[] _probs;
    public readonly T[] Items;
    public readonly int Length;

    public RandomItem(T[] items)
    {
        if(items==null || items.Length==0) throw new ArgumentNullException();
        Items = items;
        Length = items.Length;
    }

    public RandomItem(T[] items, float[] probs) : this(items)
    {
        float sum = 0;
        foreach (var f in probs)
        {
            if (f < 0 || f > 1) throw new ArgumentException("Bad probabilites");
            sum += f;
        }
        if (Math.Abs(1 - sum) > Delta) throw new ArgumentException("Probabilites' sum != 1");
        if (items.Length != probs.Length) throw new ArgumentException("Bad lengths");
        _probs = probs;
    }

    public T Next()
    {
        if (_probs != null)
        {
            float r = (float)_random.NextDouble();
            for (int i = 0; i < _probs.Length; i++)
            {
                r -= _probs[i];
                if (r < 0)
                {
                    Current = Items[i];
                    return Current;
                }
            }
            Current = Items[_probs.Length - 1];
        }
        else
            Current = Items[_random.Next(0, Items.Length)];
        return Current;
    }

    public T Current { get; private set; }

    object IEnumerator.Current => Current;
    public bool MoveNext()
    {
        Next();
        return true;
    }
    public void Reset()
    {
    }
    public void Dispose()
    {
    }

    public override string ToString()
    {
        var res = "";
        for (var i = 1; i < Length; i++)
        {
            res += Items[i] + " with probability: " + (_probs?[i] ?? 1f/Length) + "\n";
        }

        return res;
    }

}

