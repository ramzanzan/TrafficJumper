
using System;
using UnityEngine;
using UnityEngine.UI;

public class FixedJoystickPosVM : IViewModel
{
    private RectTransform _joystickMock;
    
    public FixedJoystickPosVM(Screen screen)
    {
        if(screen.Type!=Screen.ScreenType.JoystickPos) throw new ArgumentException();
        this.Screen = screen;
        screen.TurnedOn = TurnOnHandler;
        screen.Rect.transform.Find("Back").GetComponent<Button>().onClick.AddListener(HandleBackButton);
        screen.Rect.transform.Find("Confirm").GetComponent<Button>().onClick.AddListener(ConfirmPos);
        _joystickMock = screen.Rect.transform.Find("JoystickMock").transform as RectTransform;
        
        var back = screen.Rect.GetComponentInChildren<ClickHandler>();
        back.Clicked = () => { _joystickMock.position = back.EventData.position; };
    }

    private void ConfirmPos()
    {
        AppSettings.GetInstance().SetJoystick(_joystickMock.position);
    }

    private void TurnOnHandler()
    {
        _joystickMock.position = AppSettings.GetInstance().DefaultJoystickPos;
    }
    
    public Screen Screen { get; }
    public void HandleBackButton()
    {
       Screen.Back();
    }
}
