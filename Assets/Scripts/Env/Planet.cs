using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {

	[SerializeField]
	[Range(0.1f, 1.0f)]
	private float _rotationSpeed = 0;

	[SerializeField]
	private float _gravityMultipler = 0.15f;

	void FixedUpdate() {
		transform.RotateAround(transform.position, Vector3.up, _rotationSpeed);
	}

	public float GravityMultipler {
		get {
			return _gravityMultipler;
		}
	}
}
