using System;
using UnityEngine;
using UnityEngine.UI;

public class SkinsContainer : MonoBehaviour
{
    private static SkinsContainer _instance;

    public static SkinsContainer GetInstance()
    {
        if(_instance==null) throw new Exception("Not initialized yet");
        return _instance;
    }
    
    public SkinPricing[] Skins;
    public Image[] Icons;
    
    private void Start()
    {
        if(_instance!=null) throw new Exception("Second singleton");
        _instance = this;
    }
}
