using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {

	[SerializeField]
	[Range(0.1f, 1.0f)]
	private float _rotationSpeed = 0;

	[SerializeField]
	private float _gravityMultipler = 0.2f;
	
	private float _gravityRadius = 0;

	private Transform _rocket;

	void Start() {
		_rocket = GameObject.FindGameObjectWithTag(Tags.Player).transform;
		_gravityRadius = (GetComponent<SphereCollider>().radius + transform.FindChild("PlanetGravity").GetComponent<SphereCollider>().radius);
	}

	void FixedUpdate() {
		transform.RotateAround(transform.position, Vector3.up, _rotationSpeed);

		if(_rocket == null)
			_rocket = GameControler.Instance.MyRocket.transform;

		if(_gravityRadius > Vector3.Distance(transform.position, _rocket.position))
			_rocket.GetComponent<RocketControl>().EnterPlanetGravity(transform);
		else
			_rocket.GetComponent<RocketControl>().ExitPlanetGravity(transform);
	}

	public float GravityMultipler {
		get {
			return _gravityMultipler;
		}
	}
}
