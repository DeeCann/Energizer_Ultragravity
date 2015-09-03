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

	private bool _gotRocketInMyGravity = false;

	void Start() {
		_rocket = GameObject.FindGameObjectWithTag(Tags.Player).transform;
		//_gravityRadius = (transform.FindChild("PlanetGravity").GetComponent<SphereCollider>().radius);
		_gravityRadius = GetComponent<SphereCollider>().radius + (GetComponent<SphereCollider>().radius*2);
	}

	void FixedUpdate() {
		transform.RotateAround(transform.position, transform.up, _rotationSpeed);

		if(_rocket == null && GameControler.Instance.MyRocket != null)
			_rocket = GameControler.Instance.MyRocket.transform;

		if(_rocket ) {
			if(!_gotRocketInMyGravity && _gravityRadius > Vector3.Distance(transform.position, _rocket.position)) {
				_gotRocketInMyGravity = true;
				_rocket.GetComponent<RocketControl>().EnterPlanetGravity(transform);
			}

			if(_gotRocketInMyGravity && _gravityRadius < Vector3.Distance(transform.position, _rocket.position)) {
				_gotRocketInMyGravity = false;
				//if(_rocket.GetComponent<RocketControl>().IsCurrentGravityPlanet(transform))
					_rocket.GetComponent<RocketControl>().ExitPlanetGravity(transform);

			}



//			if(_gravityRadius > Vector3.Distance(transform.position, _rocket.position))
//
//			else {
//				if(_rocket.GetComponent<RocketControl>().IsCurrentGravityPlanet(transform))
//					_rocket.GetComponent<RocketControl>().ExitPlanetGravity(transform);
//			}
		}
	}

	public float GravityMultipler {
		get {
			return _gravityMultipler;
		}
	}
}
