
using System;
using UnityEngine.UI;

public class SettingsVM : IViewModel
{
    
    public SettingsVM(Screen screen)
    {
        if(screen.Type!=Screen.ScreenType.Settings) throw new ArgumentException();
        this.Screen = screen;
        screen.Rect.transform.Find("Fixed").GetComponent<Button>().onClick.AddListener(SetFixedJoystick);
        screen.Rect.transform.Find("Floating").GetComponent<Button>().onClick.
            AddListener(()=>AppSettings.GetInstance().SetJoystick(false));
        screen.Rect.transform.Find("Back").GetComponent<Button>().onClick.AddListener(HandleBackButton);
    }

    private void SetFixedJoystick()
    {
        Screen.GoTo(GUIController.GetInstance().Screens[Screen.ScreenType.JoystickPos]);    
    }
    
    public Screen Screen { get; }
    public void HandleBackButton()
    {
        Screen.Back();
    }
}
