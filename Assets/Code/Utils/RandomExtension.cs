using System;

public static class RandomExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="rnd"></param>
    /// <param name="array"></param>
    /// <param name="from"></param> inclusive
    /// <param name="to"></param> exclusive
    /// <typeparam name="T"></typeparam>
    public static void Shuffle<T>(this Random rnd, T[] array, int from, int to)
    {
        while (to > from+1)
        {
            int k = rnd.Next(from,to--);
            T temp = array[to];
            array[to] = array[k];
            array[k] = temp;
        }
    }

    public static void Shuffle<T>(this Random rnd, T[] array, int to)
    {
        rnd.Shuffle(array, 0, to);
    }

    public static void Shuffle<T>(this Random rnd, T[] array)
    {
        rnd.Shuffle(array,array.Length);
    }
}
