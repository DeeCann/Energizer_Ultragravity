﻿using UnityEngine;
using System.Collections;

public class RocketControl : MonoBehaviour {
	[SerializeField]
	private ParticleSystem _boostParticles;

	private Transform _planet;
	private Quaternion _rotationBeforeHit;

	private bool _hasCollision = false;
	private bool _isInPlanetGravity = false;
	private float _planetGravityDistance = 0;
	
	void FixedUpdate () {
		if(_hasCollision)
			return;

		if(_isInPlanetGravity)
			GetComponent<Rigidbody>().AddForce( (transform.position - _planet.transform.position) * -(_planetGravityDistance - Vector3.Distance(transform.position, _planet.position)) * _planet.GetComponent<Planet>().GravityMultipler, ForceMode.VelocityChange);

		if(InputEventHandler._isStartTouchAction)
		{
			Quaternion toRotation = Quaternion.LookRotation((InputEventHandler._currentTouchPosition - transform.position));
			if(_isInPlanetGravity)
				GetComponent<Rigidbody>().velocity = transform.forward * (3 + _planetGravityDistance - Vector3.Distance(transform.position, _planet.position));
			else
				GetComponent<Rigidbody>().velocity = transform.forward * 3;

			transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 1.5f);

			_boostParticles.Play();
		} else {
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
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		transform.rotation = _rotationBeforeHit;
		_hasCollision = false;
		Debug.Log(transform.rotation);
	}
}
