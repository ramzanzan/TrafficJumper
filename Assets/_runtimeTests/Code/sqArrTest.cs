using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;

public class sqArrTest : MonoBehaviour
{

	public RectTransform Container;
	public RectTransform Obj;
	public int Count;
	public float Offset;

	public void Test()
	{
		var rar = new RectTransform[Count];
		for (var i = 0; i < Count; ++i)
			rar[i] = (RectTransform) Instantiate(Obj);
		SquareArranger.Arrange(Container,rar,Offset);
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
