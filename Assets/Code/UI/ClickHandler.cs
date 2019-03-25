using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerClickHandler
{
    public Action Clicked;
    public PointerEventData EventData;
    public bool Interactable = true;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!Interactable) return;
        EventData = eventData;
        Clicked?.Invoke();
    }
    
}
