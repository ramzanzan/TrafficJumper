using System;
using UnityEngine.UI;

public class PostgameVM : IViewModel
{
    public PostgameVM(Screen screen)
    {
        if(screen.Type!=Screen.ScreenType.Postgame) throw new ArgumentException();
        Screen = screen;
        screen.Rect.transform.Find("Home").GetComponent<Button>().onClick.AddListener(GoToHome);
        screen.Rect.transform.Find("Restart").GetComponent<Button>().onClick.AddListener(GoToPlaying);
        screen.TurnedOn = TurnOnHandler;
    }

    private void TurnOnHandler()
    {
        var lvl = GameplayController.GetInstance().CurrentLevel;
        Screen.Rect.GetComponentInChildren<TaskContainer>().Refresh(lvl);
    }

    private void GoToHome()
    {
        Screen.GoTo(GUIController.GetInstance().Screens[Screen.ScreenType.Home]);
    }

    private void GoToPlaying()
    {
        Screen.GoTo(GUIController.GetInstance().Screens[Screen.ScreenType.Playing]);
    }
    
    public Screen Screen { get; }
    public void HandleBackButton()
    {
        GoToHome();
    }
}
