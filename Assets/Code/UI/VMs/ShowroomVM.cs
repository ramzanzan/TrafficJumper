using System;
using Code.Localization;
using UnityEngine;
using UnityEngine.UI;
using static ProgressData.DataType;

public class ShowroomVM : IViewModel
{
    private ModelViewer _mv;
    private SkinPricing[] _skins;
    private int _curIndx;
    private SkinPricing _current;
    private State _state;
    private ProgressData _progress;
    private Text _useText;
    private Button _use;
    
    public ShowroomVM(Screen screen)
    {
        if(screen.Type!=Screen.ScreenType.Showroom) throw new ArgumentException();
        Screen = screen;
        screen.Rect.transform.Find("Back").GetComponent<Button>().onClick.AddListener(HandleBackButton);
        screen.Rect.transform.Find("Left").GetComponent<ClickHandler>().Clicked = () => Next(true);
        screen.Rect.transform.Find("Right").GetComponent<ClickHandler>().Clicked = () => Next(false);
        _use = screen.Rect.transform.Find("Use").GetComponent<Button>();
        _use.onClick.AddListener(Use);
        _useText = screen.Rect.transform.Find("Use").GetComponentInChildren<Text>();
        
        screen.TurnedOn = TurnOnHandler;
        _skins = SkinsContainer.GetInstance().Skins;
        _mv=ModelViewer.GetInstance();
        _progress = ProgressData.GetInstance();
    }

    private void TurnOnHandler()
    {
        for(_curIndx=0;_curIndx<_skins.Length;++_curIndx)
            if (_mv.Current.gameObject == _skins[_curIndx].Skin)
                break;
        _current = _skins[_curIndx];
        SetState();
        
        Screen.Rect.transform.Find("Score").GetComponent<Text>().text = GlobalStatistics.GetInstance().Score.ToString();
    }

    private void Next(bool leftNotRight)
    {
        if(_mv.InProcess) return;
        _curIndx = (leftNotRight ? -1 : 1) + _curIndx ;
        _curIndx = _curIndx >= 0 ? _curIndx % _skins.Length : _skins.Length - 1;
        _current= _skins[_curIndx];
        _mv.ShowNext(leftNotRight,_current.Skin.transform);
        SetState();
    }

    private enum State
    {
        Select,
        Selected,
        Price,
        Condition
    }

    private void SetState()
    {
        var lc = LocalizationController.Instance;
        var name = _current.Skin.name;
        _use.interactable = true;
        if (AppSettings.GetInstance().AvatarSkinName == name)
        {
            _state = State.Selected;
            _useText.text = lc["selected"];
            _use.interactable = false;
        }
        else if (_progress[Skin, name] || 
                 _current.CompletedLvlCond!="" && _progress[ProgressData.DataType.Level,_current.CompletedLvlCond])
        {
            _state = State.Select;
            _useText.text = lc["select"];
        }
        else if (_current.CompletedLvlCond=="")
        {
            _state = State.Price;
            _useText.text = _current.Cost.ToString();
        }
        else
        {
            _state = State.Condition;
            _useText.text = lc["lvlCondition"] + _current.CompletedLvlCond.Substring(1);
            _use.interactable = false;
        }
    }
    
    private void Use()
    {
        switch (_state)
        {
            case State.Select:
                AppSettings.GetInstance().SetAvatar(_current.Skin.name);
                break;
            case State.Price:
                var b = LocalStore.Buy(Skin, _current.Skin.name, _current.Cost);
                if (b)
                    Screen.Rect.transform.Find("Score").GetComponent<Text>().text =
                        GlobalStatistics.GetInstance().Score.ToString();
                else
                {
                    //todo not enough
                }
                break;
        }
        SetState();
    }
    
    public Screen Screen { get; }
    public void HandleBackButton()
    {
        Screen.Back();
    }
}
