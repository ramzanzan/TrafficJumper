using System;
using System.Collections.Generic;

public static class GenUtils
{
    public static bool IsLineMasked(int line, byte mask)
    {
        if (mask == 0) return false;
        if(line>7) throw new ArgumentOutOfRangeException("line>7");
        mask <<= line;
        return (mask & 0x80) == 0x80;
    }

    public static byte MaskLine(int line)
    {
        if(line>7) throw new ArgumentException("Line is > 7");
        byte mask = (byte)(0x80 >> line);
        return mask;
    }

    public static int FreeLines(int maxLines, byte mask)
    {
        int res = maxLines;
        for (int i = 0; i < maxLines; i++)
        {
            if ((mask & 0x80) == 0x80) res--;
            mask <<= 1;
        }
        return res;
    }

    public static CarDescriptor FarthestCar(LinkedList<IDescriptorWithID> list)
    {
        CarDescriptor car = null;
        foreach (var e in list)
        {
            var descriptor = e as CarDescriptor;
            if (descriptor != null && (car == null || car.Position.y < descriptor.Position.y))
                car = descriptor;
        }
        if(car==null) throw new Exception("There is not a carDescriptor in list");
        return car;
    }
}
