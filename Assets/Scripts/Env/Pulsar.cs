using UnityEngine;
using System.Collections;

public class Pulsar : MonoBehaviour {

	public AudioClip _collectSound;
	private AudioClip _defaultSound;

	void Start() {
		_defaultSound = GetComponent<AudioSource>().clip;
	}

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent<Collider>().tag == Tags.Player) {
			GetComponent<AudioSource>().clip =_collectSound;
			GetComponent<AudioSource>().PlayOneShot(_collectSound);

			StartCoroutine(ResetSoundToDefault());
			GameControler.Instance.Points = 1;
		}
	}

	IEnumerator ResetSoundToDefault() {
		yield return new WaitForSeconds(1);
		GetComponent<AudioSource>().clip = _defaultSound;
		GetComponent<AudioSource>().Play();
	}
}
