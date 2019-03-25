using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SquareArranger
{
    public static void Arrange(RectTransform container, IEnumerable<RectTransform> items, float offset)
    {
        var conSize = new Vector2(container.rect.width,container.rect.height);
        var conS = conSize.x * conSize.y;
        var squareSide = conSize.x / (Mathf.Floor(conSize.x/ Mathf.Sqrt(conS / items.Count()))+1);
        var xAnchorDelta = squareSide / conSize.x;
        var yAnchorDelta = squareSide / conSize.y;
        var xCount = (int)(conSize.x / squareSide);
        
        var  i = 0;
        var v2offset = new Vector2(offset,offset); 
        foreach (var rt in items)
        {
            rt.SetParent(container);
            var minX = i % xCount * xAnchorDelta;
            var maxX = minX + xAnchorDelta;
            var maxY= 1-(i/xCount)*yAnchorDelta;
            var minY= maxY - yAnchorDelta;
            rt.anchorMax = new Vector2(maxX,maxY);
            rt.anchorMin = new Vector2(minX,minY);
            rt.offsetMax = -v2offset;    //todo Это странно. На деле значения смещения оказываются противоположными
            rt.offsetMin = v2offset;
            ++i;
        }

    }
}
