using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    private static GUIController _instance;
    
    public static GUIController GetInstance()
    {
        if(_instance==null) throw new Exception("bad order");
        return _instance;
    }

    public IDictionary<Screen.ScreenType,Screen> Screens { get; private set; }

    public Camera ShowroomCamera;
    public GameObject Playing;
    public GameObject Home;
    public GameObject Pause;
    public GameObject Postgame;
    public GameObject Company;
    public GameObject LevelDetails;
    public GameObject Settings;
    public GameObject JoystickPos;
    public GameObject Skins;
    public GameObject Showroom;

    public void Start()
    {
        if(_instance!=null) throw new Exception("Second singleton");
        _instance = this;
        Screen screen;
        Screens = new Dictionary<Screen.ScreenType, Screen>(10);
        
        screen= new Screen(Playing,Camera.main,Screen.ScreenType.Playing);
        new PlayingVM(screen);
        Screens.Add(screen.Type,screen);
        
        screen = new Screen(Home,Camera.main,Screen.ScreenType.Home);
        new HomeVM(screen);
        Screens.Add(screen.Type,screen);
        
        screen = new Screen(Pause,Camera.main,Screen.ScreenType.Pause);
        new PauseVM(screen);
        Screens.Add(screen.Type,screen);
        
        screen = new Screen(Postgame,Camera.main, Screen.ScreenType.Postgame);
        new PostgameVM(screen);
        Screens.Add(screen.Type,screen);
        
         screen = new Screen(Company,Camera.main, Screen.ScreenType.Company);
        new CompanyVM(screen);
        Screens.Add(screen.Type,screen);
        
        screen = new Screen(LevelDetails,Camera.main, Screen.ScreenType.LevelDetails);
        new LevelDetailsVM(screen);
        Screens.Add(screen.Type,screen);
        
        screen = new Screen(Settings,Camera.main, Screen.ScreenType.Settings);
        new SettingsVM(screen);
        Screens.Add(screen.Type,screen);
        
         screen = new Screen(JoystickPos,Camera.main, Screen.ScreenType.JoystickPos);
        new FixedJoystickPosVM(screen);
        Screens.Add(screen.Type,screen);
        
        screen = new Screen(Skins, Camera.main, Screen.ScreenType.Skins);
        new SkinsVM(screen);
        Screens.Add(screen.Type,screen);
        
        screen = new Screen(Showroom, ShowroomCamera, Screen.ScreenType.Showroom);
        new ShowroomVM(screen);
        Screens.Add(screen.Type,screen);
        
        Screens[Screen.ScreenType.Home].TurnOn();
    }
}
