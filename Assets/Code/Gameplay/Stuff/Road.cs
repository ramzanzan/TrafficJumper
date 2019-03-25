
using System;
using UnityEngine;

public class Road
{
    private static Road _instance;

    public static Road Instance
    {
        get
        {
//            if (_instance == null) throw new Exception("have not been initialized yet");
            if (_instance == null) 
                _instance = new Road(1,0);
            return _instance;
        }
    }

    public Road(float lineWidth, float posX, int linesCount = 5)
    {
        LineWidth = lineWidth;
        PositionX = -lineWidth*linesCount/2;
        if(linesCount>7) throw new ArgumentException("linesCount > 7");
        LinesCount = linesCount;
    }
    
    public readonly float LineWidth;
    public readonly float PositionX;
    public readonly float PostionY = 0;
    public readonly int LinesCount;
    public float MiddlePosition => PositionX + LinesCount * LineWidth / 2;
    public float Width => LineWidth * LinesCount;

    public int LineNumFromPosX(float x)
    {
        if (x<PositionX || x>PositionX+LineWidth*LinesCount)
            throw new ArgumentException();
        return Mathf.FloorToInt((x - PositionX) / LineWidth);
    }

    public float LinePosXFromNum(int num)
    {
        if (num >= LinesCount) throw new ArgumentException();
        return LineWidth/2 + LineWidth * num + PositionX;
    }
}
