using UnityEngine;

public class ProjectorPointer : MonoBehaviour
{
	private const float SizeToUnityMeter = 2;
	private Projector head;
	private Projector body;
	private float headHeight;
	private float distance;
	private float width;
	private float headZ;

	public void Init(float distance,float width,float headHeight)
	{
		this.distance = distance;
		this.width = width;
		this.headHeight = headHeight;
		head.orthographicSize = width / SizeToUnityMeter;
		head.aspectRatio = headHeight / width;
		headZ = headHeight / 2;
		head.transform.localPosition= new Vector3(0,0,headZ);
		body.orthographicSize = width / SizeToUnityMeter;
	}

	void Start()
	{
		head= transform.GetChild(0).GetComponent<Projector>();
		body= transform.GetChild(1).GetComponent<Projector>();
		
	}

	public void SetLoad(float power)
	{
		float headPower = headHeight / distance;
		if (power < headPower)
			power = 0;
		else
			power = power - headPower;
		head.transform.localPosition = new Vector3(0, 0, headZ + (distance) * power);
		body.transform.localPosition = new Vector3(0,0,(distance)/2 * power);
		body.aspectRatio = (distance)/width * power;
//		head.transform.localPosition = new Vector3(0, 0, headZ + (distance - headHeight) * power);
//		body.transform.localPosition = new Vector3(0,0,(distance - headHeight)/2 * power);
//		body.aspectRatio = (distance - headHeight)/width * power;
	}

	public void Resetting()
	{
		head.transform.localPosition= new Vector3(0,0,headZ);
		body.transform.localPosition = Vector3.zero;
		body.aspectRatio = 0;
	}
}
