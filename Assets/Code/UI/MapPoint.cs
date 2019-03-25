using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapPoint : MonoBehaviour, IPointerClickHandler
{
    public bool Interactable;
    public Action<string> Clicked;
    public string Dependency;
    
    public void OnPointerClick(PointerEventData eventData)
    {
       if(!Interactable) return;
       Clicked(name);
    }
}
