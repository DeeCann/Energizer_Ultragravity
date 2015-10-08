using UnityEngine;
using System.Collections;

public class Supernova : MonoBehaviour {

	private AudioSource _collectSound;

	void Start () {
		_collectSound = GetComponent<AudioSource>();
	}

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent<Collider>().tag == Tags.Player) {
			if(!_collectSound.isPlaying)
				_collectSound.Play();

			GameControler.Instance.Points = 1;
			ShipEnergy.Instance.GetExtraEnergy = 620;
		}
	}
}
