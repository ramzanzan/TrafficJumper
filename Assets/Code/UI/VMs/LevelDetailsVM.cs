using System;
  using UnityEngine.UI;

public class LevelDetailsVM : IViewModel
{
    private Text _text;
    private Scrollbar _scrollbar;
    public Screen Screen { get; }
    

    public LevelDetailsVM(Screen screen)
    {
        if(screen.Type!=Screen.ScreenType.LevelDetails) throw new ArgumentException();
        Screen = screen;
        screen.TurnedOn += TurnOnHandler;
        screen.Rect.transform.Find("Back").GetComponent<Button>().onClick.AddListener(HandleBackButton);
        screen.Rect.transform.Find("Play").GetComponent<Button>().onClick.AddListener(GoToPlaying);
        _scrollbar = screen.Rect.transform.GetComponentInChildren<Scrollbar>();
        _text = screen.Rect.transform.GetComponentInChildren<Text>();
    }

    private void ResetScrollbarHandle()
    {
        _scrollbar.value = 1;
    }

    private void TurnOnHandler()
    {
        _text.text = CompanyVM.SelectedLevel.Name;
        Screen.Rect.GetComponentInChildren<TaskContainer>().Refresh(CompanyVM.SelectedLevel);

    }

    private void GoToPlaying()
    {
        ResetScrollbarHandle();
        TrafficJumperController.GetInstance().SetLevel(CompanyVM.SelectedLevel.Name); 
        Screen.GoTo(GUIController.GetInstance().Screens[Screen.ScreenType.Playing]);
    }

    public void HandleBackButton()
    {
        ResetScrollbarHandle();
        Screen.Back();
}
}
