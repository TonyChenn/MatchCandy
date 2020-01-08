using UnityEngine;
using System.Collections;

public class DelayEnable : MonoBehaviour {

	public GameObject target;
	public float delayTime = 1.0f;

	void OnEnable()
	{
		target.SetActive(false);
		Invoke("DelayFunc", delayTime);
	}
	void DelayFunc()
	{
		target.SetActive(true);
	}
	}
