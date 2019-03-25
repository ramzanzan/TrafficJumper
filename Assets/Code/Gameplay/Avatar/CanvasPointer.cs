using UnityEngine;

public class CanvasPointer : MonoBehaviour {
    private RectTransform headRectTransform;
    private RectTransform bodyRectTransform;
    private RectTransform rectTransform;
    private float headHeight;
    private float distance;
    private float width;

    public void Init(float distance,float width,float headHeight)
    {
        this.distance = distance;
        this.width = width;
        this.headHeight = headHeight;
        rectTransform.sizeDelta = new Vector2(width, 1);
        headRectTransform.sizeDelta = new Vector2(width, headHeight);
        bodyRectTransform.sizeDelta = new Vector2(width, 0);
    }

    void Start()
    {
        rectTransform = (RectTransform)transform;
        headRectTransform = (RectTransform)transform.GetChild(0);
        bodyRectTransform = (RectTransform)transform.GetChild(1);
    }

    public void SetRotation(float angle)
    {
        rectTransform.eulerAngles = new Vector3(90, 0, angle);
    }

    public void SetLoad(float power)
    {
        float headPower = headHeight / distance;
        if (power < headPower)
            power = 0;
        else
            power = power - headPower;
        headRectTransform.localPosition = new Vector3(0, (distance) * power, 0);
        bodyRectTransform.sizeDelta = new Vector2(width, (distance) * power);
    }

    public void Resetting()
    {
        headRectTransform.localPosition = Vector3.zero;
        bodyRectTransform.sizeDelta = Vector2.zero;
//        transform.eulerAngles = new Vector3(90, 0, 0);
    }
}
