using System.Collections.Generic;
using UnityEngine;

public class GOPool
{
    private Stack<GameObject> _stack;
    private LinkedList<GameObject> _activList;
    private GameObject _template;

    public GOPool()
    {
        _stack = new Stack<GameObject>();
        _activList = new LinkedList<GameObject>();
    }
    
    public GOPool(GameObject template) : this()
    {
        ResetTemplate(template);
    }

    public void ResetTemplate(GameObject template)
    {
        _template = template;
        var go = Object.Instantiate(_template);
        go.SetActive(false);
        RecallAll();
        foreach (var o in _stack)
            Object.Destroy(o);
        _stack.Clear();
        _stack.Push(go);
    }

    public void Push(GameObject go)
    {
        go.SetActive(false);
        _activList.Remove(go);
        _stack.Push(go);
    }

    public GameObject Pop()
    {
        GameObject go;
        if (_stack.Count == 0)
        {
            go = Object.Instantiate(_template);
            _activList.AddLast(go);
            return go;
        }
        go = _stack.Pop();
        go.SetActive(true);
        _activList.AddLast(go);
        return go;
    }

    public void RecallAll()
    {
        foreach (var go in _activList)
        {
            go.SetActive(false);
            _stack.Push(go);
        }
        _activList.Clear();
    }

}
