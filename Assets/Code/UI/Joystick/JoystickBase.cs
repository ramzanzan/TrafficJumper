using System;
using UnityEngine;
using UnityEngine.EventSystems;


public abstract class JoystickBase : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [NonSerialized] public Action<float> OnTick;
    [NonSerialized] public Action<Vector2, float> OnHolding;
    [NonSerialized] public Action<Vector2, float> OnRelease;
    [NonSerialized] public Action OnDoubleTap;

    public abstract void OnDrag(PointerEventData eventData);

    public abstract void OnPointerUp(PointerEventData eventData);

    public abstract void OnPointerDown(PointerEventData eventData);

    public abstract void Rezet();
}
   