using System;
using UnityEngine;
using UnityEngine.UI;

public class SkinsVM : IViewModel
{
    private bool _firstOn;
    private ModelViewer _mv;
    
    public SkinsVM(Screen screen)
    {
        if(screen.Type!=Screen.ScreenType.Skins) throw new ArgumentException();
        _firstOn = true;
        Screen = screen;
        _mv = ModelViewer.GetInstance();
        Screen.TurnedOn = TurnOnHandler;
        Screen.Rect.transform.Find("Back").GetComponent<Button>().onClick.AddListener(HandleBackButton);
    }

    private void TurnOnHandler()
    {
        if (_firstOn)
        {
            var skins = SkinsContainer.GetInstance();
            var btns = new RectTransform[skins.Skins.Length];
            var container = Screen.Rect.transform.Find("Skins") as RectTransform;
            var template = Screen.Rect.transform.Find("Template") as RectTransform;
            for (int i = 0; i < skins.Skins.Length; ++i)
            {
                btns[i] = GameObject.Instantiate(template);
                btns[i].gameObject.SetActive(true);
                var lambda = btns[i].GetComponent<MonoLambda>();
                lambda.Parameters = skins.Skins[i].Skin.transform;
                lambda.Function = o => GoToShowroom((Transform)o);
                btns[i].GetComponent<Button>().onClick.AddListener(lambda.Call);
            }
            SquareArranger.Arrange(container,btns,0);
            _firstOn = false;
        }    
    }

    private void GoToShowroom(Transform init)
    {
        _mv.Init(init);
        Screen.GoTo(GUIController.GetInstance().Screens[Screen.ScreenType.Showroom]);
    }
    
    public Screen Screen { get; }
    public void HandleBackButton()
    {
        Screen.Back();
    }
}
