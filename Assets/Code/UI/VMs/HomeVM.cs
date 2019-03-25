using System;
using UnityEditor;
using UnityEngine;
  using UnityEngine.Events;
  using UnityEngine.EventSystems;
  using UnityEngine.UI;

public class HomeVM : IViewModel
{

    public HomeVM(Screen screen)
    {
        if(screen.Type!=Screen.ScreenType.Home) throw new ArgumentException();
        Screen = screen;
        Screen.Rect.transform.Find("Company").GetComponent<Button>().onClick.AddListener(GoToCompany);
        Screen.Rect.transform.Find("Settings").GetComponent<Button>().onClick.
            AddListener(()=>Screen.GoTo(GUIController.GetInstance().Screens[Screen.ScreenType.Settings]));
        Screen.Rect.transform.Find("Skins").GetComponent<Button>().onClick.
            AddListener(()=>Screen.GoTo(GUIController.GetInstance().Screens[Screen.ScreenType.Skins]));
        Screen.Rect.GetComponentInChildren<ClickHandler>().Clicked += GoToPlaying;
    }

    private void GoToPlaying()
    {
        TrafficJumperController.GetInstance().SetLevel("main"); 
//        TrafficJumperController.GetInstance().SetLevel("test"); 
        Screen.GoTo(GUIController.GetInstance().Screens[Screen.ScreenType.Playing]);
    }

    private void GoToCompany()
    {
        Screen.GoTo(GUIController.GetInstance().Screens[Screen.ScreenType.Company]);
    }

    public Screen Screen { get; }
    public void HandleBackButton()
    {
        throw new System.NotImplementedException();
    }
}
