using System;
using System.Collections.Generic;
using UnityEngine;

public class Screen
{
    public enum ScreenType
    {
        Home,
        Playing,
        Pause,
        Postgame,
        Company,
        LevelDetails,
        Settings,
        JoystickPos,
        Skins,
        Showroom,
        //debug
        Choosing,
        Testing,
    }
   
    public static readonly Stack<Screen> Top = new Stack<Screen>();
    
    public readonly ScreenType Type;
    public readonly GameObject Rect;
    public readonly Camera Cam;
    public Screen Previous { get; private set; }
    private bool isOver, isBlured;
    public Action TurnedOn, TurnedOff;
    public Action CamTurnedOn, CamTurnedOff;
    public Action BackHandler;
    
    public Screen(GameObject rect, Camera cam, ScreenType t)
    {
        Rect = rect;
        Cam = cam;
        Type = t;
    }
    
    public virtual void TurnOn(bool top=true)
    {
        if (top) Top.Push(this); 
        Rect.SetActive(true);
        TurnedOn?.Invoke();
    }

    public virtual void TurnOff()
    {
        Rect.SetActive(false);
        TurnedOff?.Invoke();
        if (Top.Peek() == this)
            Top.Pop();
    }

    protected virtual void TurnOnCamera()
    {
        Cam.enabled = true;
        CamTurnedOn?.Invoke();
    }

    protected virtual void TurnOffCamera()
    {
        Cam.enabled = false;
        CamTurnedOff?.Invoke();
    }

    public virtual void GoTo(Screen next)
    {
        if (isOver)
        {
            Hide();
            Previous.GoTo(next);
        }
        if (Cam!= next.Cam)
        {
            next.TurnOnCamera();
            TurnOffCamera();
        }
        TurnOff();
        next.Previous = this;
        next.TurnOn();
    }

    protected virtual void Blur()
    {
        //todo
        isBlured = true;
    }

    protected virtual void Unblur()
    {
        isBlured = false;
    }
    
    public virtual void ShowOver(Screen screen, bool blur)
    {
        if(Cam!=screen.Cam) 
           throw new ArgumentException("Cam != screen.Cam");
        screen.Previous = this;
        screen.isOver = true;
        if (blur)
        {
            screen.TurnOn();
            Blur();
        }
        else
        {
            screen.TurnOn(false);
        }
    }

    public virtual void Hide()
    {
        if(isOver!=true)
            throw new Exception("Screen isnt over");
        isOver = false;
        if(Previous.isBlured)
            Previous.Unblur();
        TurnOff();
    }
    
    public virtual void Back()
    {
        if(isOver) Hide();
        else
        {
            var prePre = Previous.Previous;
            GoTo(Previous);
            Previous.Previous = prePre;
        }

    } 
}
