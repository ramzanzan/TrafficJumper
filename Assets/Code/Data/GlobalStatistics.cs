
[System.Serializable]
public sealed class GlobalStatistics
{
    private static GlobalStatistics _instance;

    public static GlobalStatistics GetInstance()
    {
        return _instance ?? (_instance = new GlobalStatistics());
    }

    private GlobalStatistics()
    {
        
    }

    public int Score = 1000;

}
