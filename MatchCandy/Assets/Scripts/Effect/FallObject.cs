using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObject : MonoBehaviour {
	void Start () {
        Destroy(gameObject, 10);
	}

    float timer = 1;
    int randomYType;    //0 不变化     1向上     -1向下
	void Update () {
        transform.Translate(-transform.right * Time.deltaTime);
        timer += Time.deltaTime;
        if (timer > 1f)
        {
            timer = 0;
            randomYType = Random.Range(-1, 2);
        }
        else
            transform.Translate(transform.up * randomYType * Time.deltaTime/ 4);
	}
}
