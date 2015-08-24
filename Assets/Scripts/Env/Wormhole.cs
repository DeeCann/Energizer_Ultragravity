using UnityEngine;
using System.Collections;

public class Wormhole : MonoBehaviour {

	[SerializeField]
	private Transform _destination;
	
	public Transform Destination {
		get {
			return _destination;
		}
	}

	void OnTriggerEnter(Collider other) {
		if(_destination != null && other.GetComponent<Collider>().tag == Tags.Player ) {
			if(!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().Play();

			StartCoroutine(SoundFadeOut());
		}
	}

	IEnumerator SoundFadeOut() {
		while(GetComponent<AudioSource>().volume > 0.1f) {
			GetComponent<AudioSource>().volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 0, Time.deltaTime * 1);

			yield return null;
		}

		GetComponent<AudioSource>().Stop();
		GetComponent<AudioSource>().volume = 0.5f;
	}
}
