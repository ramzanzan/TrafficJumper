using System.Collections.Generic;
using UnityEngine;

public class ContentArranger : MonoBehaviour
{
    protected LinkedList<RectTransform> Content;
    protected RectTransform Rect;
    
    private void OnEnable()
                       {
                       if (Content == null)
                       {
                       Content = new LinkedList<RectTransform>();
                       Rect = (RectTransform) transform;
                       }
                       
                       }

    public virtual void Add(RectTransform rect)
                                           {
                                           rect.SetParent(Rect);
                                           rect.pivot=Vector2.up;
                                           rect.anchorMin=Vector2.up;
                                           rect.anchorMax=Vector2.one;
                                           float offsetY = Content.Count == 0
                                           ? 0
                                           : Content.Last.Value.offsetMin.y;
                                           rect.anchoredPosition=new Vector2(0,offsetY);
                                           var vec2 = rect.offsetMax;
                                           vec2.x = 0;
                                           rect.offsetMax = vec2;
                                           Content.AddLast(rect);
                                           Rect.offsetMin = rect.offsetMin;
                                           }


    public virtual void Clear()
                           {
                           Rect.offsetMin = Vector2.zero;
                           foreach(var e in Content) 
                           Destroy(e.gameObject);
                           Content.Clear();
                           }
}
