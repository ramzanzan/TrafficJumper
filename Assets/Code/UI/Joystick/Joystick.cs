using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class Joystick : JoystickBase 
{
    [Range(0f, 2f)]
    public float HandleLimit = 1f;
    [Range(0f, 1f)]
    public float DoubleTapTreshold = 0.2f;
    public RectTransform Background;
    public RectTransform Handle;

    private Vector2 _inputVector = Vector2.zero;
    private Vector2 _position;
    private float _timer;
    private bool _toTick;
    private bool _isOnHoldingPushed;
    private bool _indeterminacy;

    private void Start()
    {
        _position = transform.position;
    }

    public override void Rezet()
    {
        _toTick = false;
        _timer = 0;
        _position = transform.position;
        Handle.transform.localPosition=Vector3.zero;
    }
    
    public override void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - _position;
        _inputVector = (direction.magnitude > Background.sizeDelta.x / 2f) ? direction.normalized : direction / (Background.sizeDelta.x / 2f);
        Handle.anchoredPosition = (_inputVector * Background.sizeDelta.x / 2f) * HandleLimit;
        if (_timer > DoubleTapTreshold)
        {
            OnHolding?.Invoke(_inputVector, _timer);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        _indeterminacy = false;
        _isOnHoldingPushed = false;
        if (_timer==0)
        {
            _toTick = true;
        }
        if(_timer>0 && _timer<=DoubleTapTreshold)
        {
            OnDoubleTap?.Invoke();
            //Debug.LogWarning("DOUBLE "+_timer);
            _toTick = false;
            _timer = 0;
        }
        if(_timer>DoubleTapTreshold)
        {
            _timer = 0;
        }
        OnDrag(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (_timer <= DoubleTapTreshold)
        {
            _indeterminacy = true;
        }
        if (_timer > DoubleTapTreshold)
        {
            _toTick = false;
            OnRelease?.Invoke(_inputVector, _timer);
            //Debug.LogWarning("LONG RELEASe " + _timer);
            _timer = 0;
        }
        _inputVector = Vector2.zero;
        Handle.anchoredPosition = Vector2.zero;
    }

    private void Update()
    {
        if (_timer > DoubleTapTreshold)
        {
            OnTick?.Invoke(_timer);
            if (!_isOnHoldingPushed)
            {
                OnHolding?.Invoke(_inputVector, _timer);
                _isOnHoldingPushed = true;
               // Debug.LogWarning("Drag PUSHED");
            }
        }
    }

    private void FixedUpdate()
    {
        if (_toTick)
        {
            _timer += Time.fixedDeltaTime;
            if (_timer > DoubleTapTreshold && _indeterminacy)
            {
                _timer = 0;
                _toTick = false;
                _indeterminacy = false;
            }

        }

    }
}
