namespace Code.Localization
{
    [System.Serializable]
    public class LocalizationData 
    {
        public LocalizationItem[] Items;
    }

    [System.Serializable]
    public class LocalizationItem
    {
        public string k;    //key
        public string v;    //text
    }
}