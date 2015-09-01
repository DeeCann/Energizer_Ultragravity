using UnityEngine;
using System.Collections;

public class Ufo : MonoBehaviour {
	[SerializeField]
	[Range(-10,10)]
	private float _minHeight = 0;

	[SerializeField]
	[Range(-10, 10)]
	private float _maxHeight = 0;

	[SerializeField]
	[Range(1, 10)]
	private float speed = 1;

	void FixedUpdate () {
		transform.RotateAround(transform.position, transform.up,10);
		transform.position = new Vector3(transform.position.x,  Mathf.PingPong(Time.time * speed * 0.1f, _maxHeight-_minHeight) + _minHeight, transform.position.z);
	}
}
