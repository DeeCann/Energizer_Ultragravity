﻿using UnityEngine;
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

	private Quaternion _rotationBeforeHit;

	private bool _hasPlanetCollision = false;
	private bool _hasCollision = false;
	private bool _isInPlanetGravity = false;
	private bool _isRocketHasPulsarVelocity = false;
	private float _planetGravityDistance = 0;

	private float _currentPlanetInstanceID;

	private GameObject _flowTarget;

	void Start() {
		_engineSound = GetComponent<AudioSource>();

		_flowTarget = new GameObject();
		_flowTarget.transform.position = transform.forward + transform.position;
		_boostParticles.startSize = 0;
		_boostParticles.Play();
	}

	void FixedUpdate () {
		_flowTarget.transform.position = Vector3.Lerp( _flowTarget.transform.position, InputEventHandler._currentTouchPosition, Time.deltaTime * 1.2f);

		if(_hasCollision)
			return;

		if(_isRocketHasPulsarVelocity) {
			GetComponent<Rigidbody>().velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity, Vector3.zero, Time.deltaTime);
			return;
		}

		if(_isInPlanetGravity) {
			if(InputEventHandler._isStartTouchAction)
				GetComponent<Rigidbody>().AddForce( (transform.position - _planet.transform.position) * -(_planetGravityDistance - Vector3.Distance(transform.position, _planet.position)) * _planet.GetComponent<Planet>().GravityMultipler, ForceMode.VelocityChange);
			else
				GetComponent<Rigidbody>().AddForce( (transform.position - _planet.transform.position) * -(_planetGravityDistance - Vector3.Distance(transform.position, _planet.position)) * _planet.GetComponent<Planet>().GravityMultipler * 0.01f, ForceMode.VelocityChange);

		}

		if(InputEventHandler._isStartTouchAction)
		{

			if(Vector3.Distance(_flowTarget.transform.position, transform.position) < 0.2f)
				_flowTarget.transform.position = transform.forward + transform.position;

			if(_isInPlanetGravity)
				GetComponent<Rigidbody>().velocity = transform.forward * (1.5f + _planetGravityDistance - Vector3.Distance(transform.position, _planet.position));
			else
				GetComponent<Rigidbody>().velocity = transform.forward * 1.5f;

			transform.LookAt(_flowTarget.transform);
			_boostParticles.startSize = 0.2f;

			if(!_engineSound.isPlaying) {
				_engineSound.Play();
				StartCoroutine(EngineSoundOn());
			}

		} else {
			_flowTarget.transform.position = transform.position;
			GetComponent<Rigidbody>().velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity, Vector3.zero, Time.deltaTime);
			_boostParticles.startSize = 0;

			StartCoroutine(EngineSoundOff());
		}

	}

	void OnCollisionEnter(Collision other) {
		if(other.collider.tag == Tags.Planet) {
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
	}

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent<Collider>().tag == Tags.Asteroid) {
			_hasCollision = true;
			_boostParticles.startSize = 0;
			StartCoroutine(CollisionVelocity());
		}

		if(other.GetComponent<Collider>().tag == Tags.WormholeEnter) {
			_wormholeEnter = other.GetComponent<Wormhole>();
			_wormholeExit = other.transform.root.GetComponent<Wormhole>();
			_boostParticles.startSize = 0;
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
				GetComponent<Rigidbody>().AddForce( transform.forward * (5.5f + Vector3.Distance(transform.position, other.transform.position)), ForceMode.Impulse);
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

	IEnumerator CollisionVelocity() {
		while(GetComponent<Rigidbody>().velocity.magnitude > 1f) {
			GetComponent<Rigidbody>().velocity = Vector3.Lerp(GetComponent<Rigidbody>().velocity, Vector3.zero, Time.deltaTime);
			yield return null;
		}
		_hasCollision = false;
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
		
		GameControler.Instance.ResetSpaceShip();
	}

	IEnumerator EngineSoundOn() {
		while(_engineSound.volume < 0.45f) {
			_engineSound.volume = Mathf.Lerp(_engineSound.volume, 0.5f, Time.deltaTime * 2);

			if( !InputEventHandler._isStartTouchAction )
				yield break;
			yield return null;
		}

		_engineSound.volume = 0.5f;
		yield break;

	}

	IEnumerator EngineSoundOff() {
		while(_engineSound.volume > 0.1f) {
			_engineSound.volume = Mathf.Lerp(_engineSound.volume, 0, Time.deltaTime * 3);

			if( InputEventHandler._isStartTouchAction || _hasPlanetCollision)
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
