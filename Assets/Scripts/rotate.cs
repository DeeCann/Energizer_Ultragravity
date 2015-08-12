using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {

	public float rotationsPerMinute = 10;

	void Update () {
		transform.Rotate(0,0,-6.0f*rotationsPerMinute*Time.deltaTime,0);
	}
}
