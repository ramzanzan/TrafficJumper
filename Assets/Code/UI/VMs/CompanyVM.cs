using System;
using UnityEngine;
using UnityEngine.UI;

public class CompanyVM : IViewModel
{
    public static Level SelectedLevel;

    private readonly Transform _map;
    private readonly Color _pointOn = new Color(.2f,.5f,.2f,1);
//    private Color _pointOff = Color.gray;
    private readonly ProgressData _pd;
    
    public CompanyVM(Screen screen)
    {
        if(screen.Type!=Screen.ScreenType.Company) throw new ArgumentException();
        Screen = screen;
        screen.TurnedOn = TurnOnHandler;
        screen.Rect.transform.Find("Back").GetComponent<Button>().onClick.AddListener(HandleBackButton);
        _map = screen.Rect.transform.Find("Map");
        _pd = ProgressData.GetInstance();
        foreach (var point in _map.GetComponentsInChildren<MapPoint>())
            point.Clicked = ShowLevelDetails;
    }

    private void TurnOnHandler()
    {
        foreach (Transform point in _map)
        {
            var mp = point.GetComponent<MapPoint>();
            if (_pd[ProgressData.DataType.Level, mp.Dependency])
            {
                point.GetComponent<Image>().color = _pointOn;
                mp.Interactable = true;
            }
        }
    }

    private void ShowLevelDetails(string lvlName)
    {
        SelectedLevel = LevelContainer.GetInstance().Levels[lvlName];
        var lvlDetails = GUIController.GetInstance().Screens[Screen.ScreenType.LevelDetails];
        Screen.GoTo(lvlDetails);
    }

    public Screen Screen { get; }
    public void HandleBackButton()
    {
        Screen.Back();
    }
}
