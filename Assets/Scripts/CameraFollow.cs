using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	private Transform _rocket;

	void Start() {
		_rocket = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void FixedUpdate() {
		Vector3 followPos = new Vector3();
		followPos = Vector3.Lerp(transform.position, _rocket.transform.position + _rocket.GetComponent<Rigidbody>().velocity, Time.deltaTime * 2);
		followPos.z = -12;
		transform.position = followPos;
	}
}
