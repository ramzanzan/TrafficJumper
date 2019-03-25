using UnityEngine;

public class PlaySettings
{
//    public static PlaySettings SS;
    
    public float MinJumpDistance;
    public float MaxJumpDistance;
    public float MaxJumpHeight;
    public float HorizontalVelocity;
    public float PowerTime;
    public float EatTime;
    public float MinCarGap; 
    public float MinCarSpeed;
    public float NormalCarSpeed;
    public float MaxCarSpeed;
    public float TimeWindow;
    public float MinPursuitVelocity;
    public Road Road = Road.Instance;
//    public float CurrentPersecutorVelocity;
    private float[] _diffTableTop;   //difference between car's position and max jump distance to different lines 
    private float[] _diffTableLow;   //difference between car's position and min car's distance to different lines 

    //todo redo
    public PlaySettings()
    {
        MinJumpDistance = 1;
        MaxJumpDistance = 4f;
        MaxJumpHeight = .5f;
        HorizontalVelocity = 4;
        PowerTime = 2;
        EatTime = 1;
        MinCarGap = 1.2f;
        MinCarSpeed = 1;
        NormalCarSpeed = 1.5f;
        MaxCarSpeed = 2;
//        TimeWindow = 5.5f;
        TimeWindow = 7.5f;
        MinPursuitVelocity = 2.5f;
        InitDiffTables();
    }

    private void InitDiffTables()
    {
        _diffTableLow = new float[Road.LinesCount];
        _diffTableTop = new float[Road.LinesCount];
        for (int i = 0; i < Road.LinesCount; i++)
        {
            float diff = Mathf.Sqrt(MaxJumpDistance * MaxJumpDistance 
                                    - i * Road.LineWidth * i * Road.LineWidth);
            if (float.IsNaN(diff))
                _diffTableTop[i] = 0;
            else
                _diffTableTop[i] = diff;
            
            diff = Mathf.Sqrt(MinCarGap * MinCarGap - i * Road.LineWidth  * i * Road.LineWidth );
            if (float.IsNaN(diff))
                _diffTableLow[i] = 0;
            else
                _diffTableLow[i] = diff;
        }

    }

    public float TopBound(int fromLine, int toLine, int size = 1)
    {
//        return _diffTableTop[Mathf.Abs(fromLine - toLine)] + (size - 1) * Vehicle.LengthStatic();
        var top = _diffTableTop[Mathf.Abs(fromLine - toLine)];
        top = top == 0 ? top : top + Vehicle.LengthStatic(size-1);
        return top;
    }
    
    public float LowerBound(int fromLine, int toLine, int size = 1)
    {
//        return _diffTableLow[Mathf.Abs(fromLine - toLine)] + (blockCount - 1) * Vehicle.LengthStatic();
        var low = _diffTableLow[Mathf.Abs(fromLine - toLine)];
        low = low == 0 ? low : low + Vehicle.LengthStatic(size);
        return low;
    }

    //todo ?
    public void Reset()
    {
        
    }
}
