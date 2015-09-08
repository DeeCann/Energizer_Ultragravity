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
	[SerializeField]
	private AudioClip _powerUp;
	[SerializeField]
	private AudioClip _ping;

	private GameObject _shield;

	private Transform _planet;
	private Wormhole _wormholeEnter;
	private Wormhole _wormholeExit;
	private Blackhole _blackHole;

	private bool _hasPlanetCollision = false;
	private bool _hasCollision = false;
	private bool _hasWormhole = false;
	private bool _hasBlackhole = false;
	private bool _isInPlanetGravity = false;
	private bool _isRocketHasPulsarVelocity = false;
	private float _planetGravityDistance = 0;

	private float _currentPlanetInstanceID;

	void Start() {
		_engineSound = GetComponent<AudioSource>();
		_boostParticles.startSize = 0;
		_boostParticles.Play();
		_shield = transform.FindChild("Rocketshield").gameObject;
	}

	void FixedUpdate () {
		if(GameControler.Instance.IsLevelSuccess) {
			StartCoroutine(EngineSoundOff());
			_boostParticles.startSize = 0;
			GetComponent<Rigidbody>().velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity, Vector3.zero, Time.deltaTime * 0.7f);
			return;
		}

		if(_hasCollision || _hasWormhole)
			return;

		if(_hasPlanetCollision && GameControler.Instance.IsShieldActive)
			return;

		if(_isRocketHasPulsarVelocity) {
			GetComponent<Rigidbody>().velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity, transform.forward * 3f, Time.deltaTime);

			Vector3 pos = InputEventHandler._currentTouchPosition-transform.position;
			var newRot = Quaternion.LookRotation(pos);

			transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * 2);
			transform.position = new Vector3(0, Mathf.Clamp(transform.position.y, -17, 17), transform.position.z);
			return;
		} 



		GetComponent<Rigidbody>().velocity = transform.forward * 3f;
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

			transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * 2);
		} else {
			if(_isInPlanetGravity) {
				float _gravityPositionMultipier = (_planetGravityDistance - Vector3.Distance(transform.position, _planet.position))/_planet.GetComponent<Planet>().GravityMultipler * 0.001f;

				GetComponent<Rigidbody>().AddForce( (transform.position - _planet.transform.position) * -(_planetGravityDistance - Vector3.Distance(transform.position, _planet.position)) * _gravityPositionMultipier, ForceMode.VelocityChange);
				GetComponent<Rigidbody>().velocity += transform.forward * -(_planetGravityDistance - Vector3.Distance(transform.position, _planet.position) * 20) * _gravityPositionMultipier;

				Vector3 pos = _planet.transform.position-transform.position;
				var newRot = Quaternion.LookRotation(pos);

				transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * 0.5f);
			} else
				transform.LookAt(transform.position + transform.forward);
		}
		transform.position = new Vector3(0, Mathf.Clamp(transform.position.y, -17, 17), transform.position.z);
	}

	void OnCollisionEnter(Collision other) {
		if(other.collider.tag == Tags.Planet && !GameControler.Instance.IsShieldActive) {
			DestroyShip();
		}

		if(GameControler.Instance.IsShieldActive) {
			_hasPlanetCollision = true;
			StartCoroutine(PLanetCollisionWithShield());
			GetComponent<Rigidbody>().AddForce( (transform.forward  ) * -2, ForceMode.Impulse );
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent<Collider>().tag == Tags.Asteroid && !GameControler.Instance.IsShieldActive) {
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

		if(other.GetComponent<Collider>().tag == Tags.Blackhole && !GameControler.Instance.IsShieldActive) {
			_blackHole = other.GetComponent<Blackhole>();
			_boostParticles.Stop();
			_hasBlackhole = true;
			StartCoroutine(EngineSoundOff());
			StartCoroutine(BlackholeEnter());
		}

		if(other.GetComponent<Collider>().tag == Tags.Pulsar) {
			if(!_isRocketHasPulsarVelocity) {
				_isRocketHasPulsarVelocity = true;
				//GetComponent<Rigidbody>().AddForce( transform.forward * (10.5f + Vector3.Distance(transform.position, other.transform.position)), ForceMode.Impulse);
				GetComponent<Rigidbody>().velocity = transform.forward * 10;

				StartCoroutine(DisablePulsarVelocity());
			}
		}
	}

	public void EnterPlanetGravity(Transform _enteredPlanet) {
		//if(!_isInPlanetGravity) {
			//Debug.Log("ENTER: " + _enteredPlanet.GetInstanceID());
			_currentPlanetInstanceID = _enteredPlanet.GetInstanceID();
			_planetGravityDistance = Vector3.Distance(transform.position, _enteredPlanet.position);
			//Debug.Log(_planetGravityDistance);
			_isInPlanetGravity = true;
			_planet = _enteredPlanet;
		//}
	}

	public bool IsCurrentGravityPlanet(Transform _currentPlanet) {
		//Debug.Log(_currentPlanetInstanceID + " - " + _currentPlanet.GetInstanceID());
		if(_currentPlanetInstanceID == _currentPlanet.GetInstanceID())
			return true;
		else
			return false;
	}

	public void ExitPlanetGravity(Transform _exitPlanet) {
		//if(_isInPlanetGravity) {
			//Debug.Log("EXIT: " + _currentPlanetInstanceID + " - " + _exitPlanet.GetInstanceID());
			if(_currentPlanetInstanceID == _exitPlanet.GetInstanceID()) {
				_currentPlanetInstanceID = 0;
				_planetGravityDistance = 0;
				_isInPlanetGravity = false;
				_planet = null;
			}
		//}
	}

	public void DestroyShip() {
		_hasPlanetCollision = true;
		_engineSound.volume = 0.5f;

		GetComponent<AudioSource>().clip = _destroySound;
		if(!GetComponent<AudioSource>().isPlaying)
			GetComponent<AudioSource>().Play();

		_boostParticles.startSize = 0;
		_collisionParticles.transform.parent = null;
		_collisionParticles.Emit(100);
		Destroy(gameObject, 0.4f);
		
		transform.FindChild("ROCKET").GetComponent<MeshRenderer>().enabled = false;
		
		GameControler.Instance.ResetSpaceShip();
	}

	public void ActivateShield() {
		_shield.SetActive(true);
		_shield.GetComponent<AudioSource>().Play();
		_shield.GetComponent<Animator>().SetBool("On", true);
		StartCoroutine(ShieldOn());
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
			transform.position = Vector3.MoveTowards(transform.position, _wormholeExit.Destination.transform.position, Time.deltaTime * 10.5f);

			yield return null;
		}

		transform.position = _wormholeExit.Destination.transform.position;
		StartCoroutine(WormholeExit());
		yield break;
	}

	IEnumerator WormholeExit() {
		GetComponent<CapsuleCollider>().enabled = true;
		while(transform.localScale.magnitude < 1.7f) {
			GetComponent<Rigidbody>().velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity, transform.forward * 3f, Time.deltaTime * 10);
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 3);
			transform.rotation = Quaternion.LookRotation(Vector3.forward);
			yield return null;
		}


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
		while(_engineSound.volume < 0.2f) {
			_engineSound.volume = Mathf.Lerp(_engineSound.volume, 0.2f, Time.deltaTime * 5);

			if( !InputEventHandler._isStartTouchAction || _hasCollision)
				yield break;
			yield return null;
		}

		_engineSound.volume = 0.02f;
		yield break;

	}

	IEnumerator EngineSoundOff() {
		while(_engineSound.volume > 0.01f) {
			_engineSound.volume = Mathf.Lerp(_engineSound.volume, 0, Time.deltaTime * 5);

			if(!_hasCollision && !_hasBlackhole)
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

	IEnumerator PLanetCollisionWithShield() {
		yield return new WaitForSeconds(1);

		_hasPlanetCollision = false;
	}

	IEnumerator ShieldOn() {
		GameControler.Instance.IsShieldActive = true;
		yield return new WaitForSeconds(7);
		
		_shield.GetComponent<AudioSource>().clip = _ping;
		_shield.GetComponent<AudioSource>().loop = true;
		_shield.GetComponent<AudioSource>().Play();
		
		yield return new WaitForSeconds(3);
		
		_shield.GetComponent<AudioSource>().Stop();
		_shield.GetComponent<AudioSource>().clip = _powerUp;
		_shield.GetComponent<AudioSource>().loop = false;
		
		_shield.GetComponent<Animator>().SetBool("On", false);
		_shield.GetComponent<Animator>().SetBool("Off", true);
		
		yield return new WaitForSeconds(0.5f);
		_shield.SetActive(false);
		GameControler.Instance.ClearCollectedEnergyPoints();
		GameControler.Instance.IsShieldActive = false;
	}

}
