using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayingVM : IViewModel
{
    private GameplayController _gm;
    private readonly GUIController _gui;
    private readonly Text _score, _distance;

    public PlayingVM(Screen view)
     {
         if (view.Type != Screen.ScreenType.Playing) throw new ArgumentException();
         Screen = view;
         _distance = view.Rect.transform.Find("Distance").GetComponent<Text>();
         _score = view.Rect.transform.Find("Score").GetComponent<Text>();
         view.Rect.transform.Find("Pause").GetComponent<Button>().onClick.AddListener(GoToPause);
         Screen.TurnedOn += TurnOnHandler;
     }

    private void ScoreChangedHandler(int score)
    {
        _score.text = score.ToString();
    }

    private void DistanceChangedHandler(float dist)
    {
        _distance.text = ((int) dist).ToString();
    }

    private void DiedHandler()
    {
        Screen.GoTo(GUIController.GetInstance().Screens[Screen.ScreenType.Postgame]);
    }
    
    private void GoToPause()
    {
        _gm.Pause();
        Screen.GoTo(GUIController.GetInstance().Screens[Screen.ScreenType.Pause]);
    }

    private void TurnOnHandler()
    {
        if (_gm == null)
        {
            _gm = GameplayController.GetInstance();
            _gm.ScoreChanged+= ScoreChangedHandler;
            _gm.DistanceChanged += DistanceChangedHandler;
            _gm.Died += DiedHandler;
        }
        if (Screen.Previous.Type == Screen.ScreenType.Home
            || Screen.Previous.Type == Screen.ScreenType.Choosing
            || Screen.Previous.Type == Screen.ScreenType.Postgame
            || Screen.Previous.Type == Screen.ScreenType.LevelDetails)
        {
            _gm.PrepareLevel();
            _gm.StartLevel();
        }
        if (Screen.Previous.Type == Screen.ScreenType.Pause)
        {
            _gm.Unpause();
        }
    }

    public Screen Screen { get; }

    public void HandleBackButton()
    {
        GoToPause();
    }
}
