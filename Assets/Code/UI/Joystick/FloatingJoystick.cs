using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick: JoystickBase 
{
    [Range(0f, 2f)]
    public float HandleLimit = 1f;
    [Range(0f, 1f)]
    public float DoubleTapTreshold = 0.2f;
    public RectTransform Handle;
   
    private Vector2 _inputVector = Vector2.zero;
    private float _timer;
    private bool _toTick;
    private bool _isOnHoldingPushed;
    private bool _indeterminacy;

    private float _screenWidth, _screenHeight, _anchorMaxY, _sizeDeltaX;
    private Vector2 _handleAnchorPos;
    private RectTransform _rect;

    private void Start()
    {
        _rect = (RectTransform) transform;
        _anchorMaxY = _rect.anchorMax.y;
        _sizeDeltaX = _rect.sizeDelta.x;
        //todo delete?
//        _screenWidth = UnityEngine.Screen.currentResolution.width;
//        _screenHeight= UnityEngine.Screen.currentResolution.height;
        _screenWidth = UnityEngine.Screen.width;
        _screenHeight= UnityEngine.Screen.height;
    }

    public override void Rezet()
    {
        _toTick = false;
        _timer = 0;
        Handle.transform.localPosition=Vector3.zero;
    }
    
    public override void OnDrag(PointerEventData eventData)
    {
        _inputVector = (eventData.position - _handleAnchorPos).normalized;
        Handle.anchoredPosition = eventData.position - _handleAnchorPos;
        if (_timer > DoubleTapTreshold)
            OnHolding?.Invoke(_inputVector, _timer);
    }

    public override void OnPointerDown(PointerEventData ea)
    {
        _handleAnchorPos = ea.position;
        Handle.anchorMin = Handle.anchorMax =
            new Vector2(ea.position.x / _screenWidth, ea.position.y / _screenHeight / _anchorMaxY);
        
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
        OnDrag(ea);
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