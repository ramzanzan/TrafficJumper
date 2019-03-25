using System;
using Code.Localization;
using UnityEngine;

public class TrafficJumperController : MonoBehaviour
{
    private static TrafficJumperController _instance;
    public static TrafficJumperController GetInstance()
    {
        if(_instance==null) throw new Exception("Сlasses initialization order have violated");
        return _instance;
    }

    private void Start()
    {
        if(_instance!=null) throw new Exception("Second singleton");
        _instance = this;
        
        _lvlContainer = LevelContainer.GetInstance();
        _casualStyle.CommonAssets = _commonAssets;
        
        _avaCtrl = AvatarController.GetInstance();
        _buildCtrl = BuildController.GetInstance();
        _buildCtrl.SetStyle(_casualStyle);
        _taskCtrl = TaskController.GetInstance();
        _taskCtrl.Initialize(); 
        GameplayController.InitializeInstance();
        _gameplayCtrl= GameplayController.GetInstance();
        
        LocalizationController.Instance.ChangeLocale(LocalizationController.Locale.EN);

        var a = AppSettings.GetInstance();

    }

    [SerializeField]
    private CommonAssets _commonAssets;
    [SerializeField]
    private StyleAssets _casualStyle;

    private LevelContainer _lvlContainer;
    private GameplayController _gameplayCtrl;
    private BuildController _buildCtrl;
    private TaskController _taskCtrl;
    private AvatarController _avaCtrl;
    
    public CommonAssets CommonAssets => _commonAssets;
    public StyleAssets CasualStyleAssets => _casualStyle;

    public void SetLevel(string name)
    {
        _gameplayCtrl.SetLevel(_lvlContainer.Levels[name]);
    }
   
}
