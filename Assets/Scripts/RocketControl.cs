using UnityEngine;
using System.Collections;

public class RocketControl : MonoBehaviour {
	[SerializeField]
	private ParticleSystem _boostParticles;

	[SerializeField]
	private ParticleSystem _collisionParticles;
	
	[SerializeField]
	private AudioClip _destroySound;
	private AudioSource _engineSound;

	private Transform _planet;
	private Wormhole _wormholeEnter;
	private Wormhole _wormholeExit;
	private Blackhole _blackHole;

	private bool _hasPlanetCollision = false;
	private bool _hasCollision = false;
	private bool _hasWormhole = false;
	private bool _isInPlanetGravity = false;
	private bool _isRocketHasPulsarVelocity = false;
	private float _planetGravityDistance = 0;

	private float _currentPlanetInstanceID;

	void Start() {
		_engineSound = GetComponent<AudioSource>();
		_boostParticles.startSize = 0;
		_boostParticles.Play();
	}

	void FixedUpdate () {
		if(_hasCollision || _hasWormhole)
			return;

		if(_isRocketHasPulsarVelocity) {
			GetComponent<Rigidbody>().velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity, Vector3.zero, Time.deltaTime);
			transform.position = new Vector3(0, transform.position.y, transform.position.z);
			return;
		}

		GetComponent<Rigidbody>().velocity = transform.forward * 2f;
		_boostParticles.startSize = 0.2f;

		if(!_engineSound.isPlaying) {
			_engineSound.Play();
			StartCoroutine(EngineSoundOn());
		}

		if(InputEventHandler._isStartTouchAction){
			Vector3 pos = InputEventHandler._currentTouchPosition-transform.position;
			var newRot = Quaternion.LookRotation(pos);

			if(_isInPlanetGravity) {
				float _gravityPositionMultipier = (_planetGravityDistance - Vector3.Distance(transform.position, _planet.position))/_planet.GetComponent<Planet>().GravityMultipler * 0.001f;

				GetComponent<Rigidbody>().AddForce( (transform.position - _planet.transform.position) * -(_planetGravityDistance - Vector3.Distance(transform.position, _planet.position)) * _gravityPositionMultipier, ForceMode.VelocityChange);
				GetComponent<Rigidbody>().velocity += transform.forward * -(_planetGravityDistance - Vector3.Distance(transform.position, _planet.position) * 20) * _gravityPositionMultipier;
			}

			transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime);
		} else {
			if(_isInPlanetGravity) {
				float _gravityPositionMultipier = (_planetGravityDistance - Vector3.Distance(transform.position, _planet.position))/_planet.GetComponent<Planet>().GravityMultipler * 0.001f;

				GetComponent<Rigidbody>().AddForce( (transform.position - _planet.transform.position) * -(_planetGravityDistance - Vector3.Distance(transform.position, _planet.position)) * _gravityPositionMultipier, ForceMode.VelocityChange);
				GetComponent<Rigidbody>().velocity += transform.forward * -(_planetGravityDistance - Vector3.Distance(transform.position, _planet.position) * 20) * _gravityPositionMultipier;

				Vector3 pos = _planet.transform.position-transform.position;
				var newRot = Quaternion.LookRotation(pos);

				transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime );
			} else
				transform.LookAt(transform.position + transform.forward);
		}
		transform.position = new Vector3(0, transform.position.y, transform.position.z);
	}

	void OnCollisionEnter(Collision other) {
		if(other.collider.tag == Tags.Planet) {
			DestroyShip();
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent<Collider>().tag == Tags.Asteroid) {
			_hasCollision = true;
			_boostParticles.startSize = 0;
			StartCoroutine(EngineSoundOff());
			StartCoroutine(CollisionVelocity());
		}

		if(other.GetComponent<Collider>().tag == Tags.WormholeEnter) {
			_hasWormhole = true;
			_wormholeEnter = other.GetComponent<Wormhole>();
			_wormholeExit = other.transform.root.GetComponent<Wormhole>();
			_boostParticles.startSize = 0;
			GetComponent<CapsuleCollider>().enabled = false;
			StartCoroutine(WormholeEnter());
		}

		if(other.GetComponent<Collider>().tag == Tags.Blackhole) {
			_blackHole = other.GetComponent<Blackhole>();
			_boostParticles.Stop();
			StartCoroutine(BlackholeEnter());
		}

		if(other.GetComponent<Collider>().tag == Tags.Pulsar) {
			if(!_isRocketHasPulsarVelocity) {
				_isRocketHasPulsarVelocity = true;
				_blackHole = other.GetComponent<Blackhole>();
				GetComponent<Rigidbody>().AddForce( transform.forward * (10.5f + Vector3.Distance(transform.position, other.transform.position)), ForceMode.Impulse);
				StartCoroutine(DisablePulsarVelocity());
			}
		}
	}

	public void EnterPlanetGravity(Transform _enteredPlanet) {
		if(!_isInPlanetGravity) {
			_currentPlanetInstanceID = _enteredPlanet.GetInstanceID();
			_planetGravityDistance = Vector3.Distance(transform.position, _enteredPlanet.position);
			_isInPlanetGravity = true;
			_planet = _enteredPlanet;
		}
	}

	public void ExitPlanetGravity(Transform _exitPlanet) {
		if(_isInPlanetGravity) {
			if(_currentPlanetInstanceID == _exitPlanet.GetInstanceID()) {
				_currentPlanetInstanceID = 0;
				_planetGravityDistance = 0;
				_isInPlanetGravity = false;
				_planet = null;
			}
		}
	}

	public void DestroyShip() {
		_hasPlanetCollision = true;
		_engineSound.volume = 0.5f;
		
		GetComponent<AudioSource>().clip = _destroySound;
		GetComponent<AudioSource>().Play();
		
		_boostParticles.startSize = 0;
		_collisionParticles.transform.parent = null;
		_collisionParticles.Emit(100);
		Destroy(gameObject, 0.4f);
		
		transform.FindChild("ROCKET").GetComponent<MeshRenderer>().enabled = false;
		
		GameControler.Instance.ResetSpaceShip();
	}

	IEnumerator CollisionVelocity() {

		while(GetComponent<Rigidbody>().velocity.magnitude > 0.2f) {
			GetComponent<Rigidbody>().velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity, Vector3.zero, Time.deltaTime * 0.7f);
			yield return null;
		}

		_hasCollision = false;
	}

	IEnumerator WormholeEnter() {
		while(transform.position.z < _wormholeExit.Destination.transform.position.z) {
			GetComponent<Rigidbody>().velocity += (_wormholeExit.Destination.transform.position - transform.position).normalized * 0.2f;
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 5);
			transform.position = Vector3.MoveTowards(transform.position, _wormholeExit.Destination.transform.position, Time.deltaTime * 0.1f);

			yield return null;
		}

		transform.position = _wormholeExit.Destination.transform.position;
		StartCoroutine(WormholeExit());
		yield break;
	}

	IEnumerator WormholeExit() {
		while(transform.localScale.magnitude < 1.7f) {
			GetComponent<Rigidbody>().velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity, transform.forward * 2f, Time.deltaTime);
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 3);
			transform.rotation = Quaternion.LookRotation(Vector3.forward);
			yield return null;
		}

		GetComponent<CapsuleCollider>().enabled = true;
		GetComponent<Rigidbody>().velocity = transform.forward * 2f;
		_hasWormhole = false;
		transform.localScale = Vector3.one;
	}

	IEnumerator BlackholeEnter() {
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		while(transform.localScale.magnitude > 0.3f) {
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 3);
			transform.position = Vector3.Lerp(transform.position, _blackHole.transform.position, Time.deltaTime * 3);
			yield return null;
		}
		
		GameControler.Instance.ResetSpaceShip();
	}

	IEnumerator EngineSoundOn() {
		while(_engineSound.volume < 0.45f) {
			_engineSound.volume = Mathf.Lerp(_engineSound.volume, 0.5f, Time.deltaTime * 2);

			if( !InputEventHandler._isStartTouchAction || _hasCollision)
				yield break;
			yield return null;
		}

		_engineSound.volume = 0.5f;
		yield break;

	}

	IEnumerator EngineSoundOff() {
		while(_engineSound.volume > 0.1f) {
			_engineSound.volume = Mathf.Lerp(_engineSound.volume, 0, Time.deltaTime * 3);

			if(!_hasCollision)
				if( InputEventHandler._isStartTouchAction || _hasPlanetCollision )
					yield break;
			yield return null;
		}
		
		_engineSound.volume = 0;
		_engineSound.Stop();

		yield break;
	}

	IEnumerator DisablePulsarVelocity() {
		yield return new WaitForSeconds(2);

		_isRocketHasPulsarVelocity = false;
	}

}
