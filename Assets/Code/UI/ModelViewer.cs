using System;
using System.Collections.ObjectModel;
using UnityEngine;

public class ModelViewer : MonoBehaviour
{
    private static ModelViewer _instance;

    public static ModelViewer GetInstance()
    {
        if(_instance==null) throw new Exception("Not initialized yet");
        return _instance;
    }
    
    private void Start()
    {
        if(_instance!=null) throw new Exception("Second singleton");
        _instance = this;
    }
    
    public float TransitionDist;
    public float TransitionTime;
    public Transform Current => _curr;
    public Transform Pedestal;
    public Vector3 ModelOffset;
    public Vector3 ModelRotation;
//    public float ZRounding;
//    public AnimationCurve Curve;
    
    private bool _hasPedestal;
    private Transform _nextPedestal;
    private Transform _curr, _next;
    private bool _leftNotRight;
    public bool InProcess { get; private set; }
    private float _k1;
    private float _k2; // -k1*t^2+k2 factors
    private float _t0;
    private float _sign;

    public void Init(Transform init)
    {
        if(_curr!=null) _curr.gameObject.SetActive(false);
        if (InProcess)
        {
            InProcess = false;
            _next.gameObject.SetActive(false);
        }
        init.position = transform.position + ModelOffset;
        init.rotation = Quaternion.Euler(ModelRotation);
        _curr = init;
        _curr.gameObject.SetActive(true);
        _hasPedestal = Pedestal != null;
        if (_hasPedestal && _nextPedestal == null)
        {
            _nextPedestal = Instantiate(Pedestal,transform);
        }

        _k1 = -6 * TransitionDist / Mathf.Pow(TransitionTime, 3);
        _k2 = Mathf.Pow(TransitionTime * Mathf.Sqrt(-_k1) / 2,2 );
    }
    
    public bool ShowNext(bool fromLeftNotRight, Transform next)
    {
        if(InProcess) return false;
        _next = next;
        _next.gameObject.SetActive(true);
        _next.rotation = _curr.rotation;
        _sign = fromLeftNotRight ? 1 : -1;
        var offset = new Vector3( -_sign * TransitionDist, 0);
        _next.position = _curr.position + offset;
        InProcess = true;
        _t0 = Time.time;
        if(_hasPedestal)
            _nextPedestal.position = Pedestal.position + offset;
        return false;
    }

    private void Update()
    {
        if (!InProcess) return;
        var t = Time.time - _t0;
        var u = _k1 * (t - TransitionTime / 2)*(t - TransitionTime / 2) + _k2;
        var d = new Vector3(_sign * u * Time.deltaTime, 0);
        _curr.position += d;
        _next.position += d;
//        var zPrev = Curve.Evaluate(t / TransitionTime) * ZRounding;
//        var zNext = Curve.Evaluate(1 - t / TransitionTime) * ZRounding;
//        _prev.position = new Vector3(_prev.position.x,_prev.position.y,zPrev);
//        _next.position = new Vector3(_next.position.x,_next.position.y,zNext);
        if (_hasPedestal)
        {
            Pedestal.position += d;
//            Pedestal.position= new Vector3(Pedestal.position.x,Pedestal.position.y,zPrev);
            _nextPedestal.position += d;
//            _nextPedestal.position= new Vector3(_nextPedestal.position.x,_nextPedestal.position.y,zPrev);
        }
        InProcess = t < TransitionTime;
        if (InProcess) return;
        _next.position = transform.position + ModelOffset;
        _curr.gameObject.SetActive(false);
        _curr = _next;
            
        if (!_hasPedestal) return;
        var tmp = Pedestal;
        Pedestal = _nextPedestal;
        _nextPedestal = tmp;
    }
}
