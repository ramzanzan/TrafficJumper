using UnityEngine.UI;

public class PauseVM : IViewModel
{

    public PauseVM(Screen pause)
    {
        Screen = pause;
        Screen.TurnedOn += TurnOnHandler;
        pause.Rect.transform.Find("Unpause").GetComponent<Button>().onClick.AddListener(HandleBackButton);
        pause.Rect.transform.Find("Home").GetComponent<Button>().onClick.AddListener(GoToHome);
    }

    private void TurnOnHandler()
    {
        var lvl = GameplayController.GetInstance().CurrentLevel;
        Screen.Rect.transform.GetComponentInChildren<Scrollbar>().value = 1;
        Screen.Rect.GetComponentInChildren<TaskContainer>().Refresh(lvl);
    }

    private void GoToHome()
    {
       GameplayController.GetInstance().EndLevel();
       GameplayController.GetInstance().Unpause();
       Screen.GoTo(GUIController.GetInstance().Screens[Screen.ScreenType.Home]); 
    }
    
    public Screen Screen { get; }

    public void HandleBackButton()
    {
        Screen.GoTo(GUIController.GetInstance().Screens[Screen.ScreenType.Playing]);
    }
}
