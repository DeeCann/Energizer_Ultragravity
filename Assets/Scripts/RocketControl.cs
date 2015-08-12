using UnityEngine;
using System.Collections;

public class RocketControl : MonoBehaviour {
	[SerializeField]
	private ParticleSystem _boostParticles;

	private Transform _planet;
	private Wormhole _wormholeEnter;
	private Wormhole _wormholeExit;
	private Blackhole _blackHole;

	private Quaternion _rotationBeforeHit;

	private bool _hasCollision = false;
	private bool _isInPlanetGravity = false;
	private float _planetGravityDistance = 0;

	private GameObject _flowTarget;

	void Start() {
		_flowTarget = new GameObject();
		_flowTarget.transform.position = transform.forward + transform.position;
	}

	void FixedUpdate () {
		_flowTarget.transform.position = Vector3.Lerp( _flowTarget.transform.position, InputEventHandler._currentTouchPosition, Time.deltaTime * 1.2f);


		if(_hasCollision)
			return;

		if(_isInPlanetGravity)
			GetComponent<Rigidbody>().AddForce( (transform.position - _planet.transform.position) * -(_planetGravityDistance - Vector3.Distance(transform.position, _planet.position)) * _planet.GetComponent<Planet>().GravityMultipler, ForceMode.VelocityChange);

		if(InputEventHandler._isStartTouchAction)
		{

			if(Vector3.Distance(_flowTarget.transform.position, transform.position) < 0.2f)
				_flowTarget.transform.position = transform.forward + transform.position;

			if(_isInPlanetGravity)
				GetComponent<Rigidbody>().velocity = transform.forward * (3 + _planetGravityDistance - Vector3.Distance(transform.position, _planet.position));
			else
				GetComponent<Rigidbody>().velocity = transform.forward * 3;

			transform.LookAt(_flowTarget.transform);
			_boostParticles.Play();
		} else {
			_flowTarget.transform.position = transform.position;
			GetComponent<Rigidbody>().velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity, Vector3.zero, Time.deltaTime);
			_boostParticles.Stop();
		}
	}

	void OnCollisionEnter(Collision other) {
		if(other.collider.tag == Tags.Planet) {
			_hasCollision = true;
			_rotationBeforeHit = transform.rotation;
			GetComponent<Rigidbody>().velocity = (other.contacts[0].point - other.collider.transform.position) * 3;
			_boostParticles.Stop();
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			StartCoroutine(CollisionVelocity());
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent<Collider>().tag == Tags.PlanetGravity) {
			_planetGravityDistance = Vector3.Distance(transform.position, other.transform.position);
			_isInPlanetGravity = true;
			_planet = other.transform.root.transform;
		}

		if(other.GetComponent<Collider>().tag == Tags.Asteroid) {
			_hasCollision = true;
			_boostParticles.Stop();
			StartCoroutine(CollisionVelocity());
		}

		if(other.GetComponent<Collider>().tag == Tags.WormholeEnter) {
			_wormholeEnter = other.GetComponent<Wormhole>();
			_wormholeExit = other.transform.root.GetComponent<Wormhole>();
			_boostParticles.Stop();
			StartCoroutine(WormholeEnter());
		}

		if(other.GetComponent<Collider>().tag == Tags.Blackhole) {
			_blackHole = other.GetComponent<Blackhole>();
			_boostParticles.Stop();
			StartCoroutine(BlackholeEnter());
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.GetComponent<Collider>().tag == Tags.PlanetGravity) {
			_isInPlanetGravity = false;
		}
	}

	IEnumerator CollisionVelocity() {
		while(GetComponent<Rigidbody>().velocity.magnitude > 1f) {
			GetComponent<Rigidbody>().velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity, Vector3.zero, Time.deltaTime);

			transform.rotation = Quaternion.Lerp(transform.rotation, _rotationBeforeHit, Time.deltaTime * 3);
			yield return null;
		}
		//GetComponent<Rigidbody>().velocity = Vector3.zero;
		_hasCollision = false;
		Debug.Log(transform.rotation);
	}

	IEnumerator WormholeEnter() {
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		while(transform.localScale.magnitude > 0.3f) {
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 3);
			transform.position = Vector3.Lerp(transform.position, _wormholeEnter.transform.position, Time.deltaTime * 3);
			yield return null;
		}

		transform.position = _wormholeExit.Destination.transform.position;
		StartCoroutine(WormholeExit());
		yield break;
	}

	IEnumerator WormholeExit() {
		while(transform.localScale.magnitude < 0.95f) {
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 2);
			yield return null;
		}
		
		transform.localScale = Vector3.one;

		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
	}

	IEnumerator BlackholeEnter() {
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		while(transform.localScale.magnitude > 0.3f) {
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 3);
			transform.position = Vector3.Lerp(transform.position, _blackHole.transform.position, Time.deltaTime * 3);
			yield return null;
		}
		
		GameControler.Instance.LevelFailed();
	}

}
