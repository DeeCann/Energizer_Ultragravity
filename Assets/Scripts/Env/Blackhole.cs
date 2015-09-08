using UnityEngine;
using System.Collections;

public class Blackhole : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		if(other.GetComponent<Collider>().tag == Tags.Player && !GameControler.Instance.IsShieldActive) {
			GetComponent<AudioSource>().Play();
			StartCoroutine(FallOff());

			Destroy(other.gameObject, 2);
		}
	}

	IEnumerator FallOff() {
		while(GetComponent<AudioSource>().volume > 0.1f) {
			GetComponent<AudioSource>().volume -= 0.8f * Time.deltaTime;
			yield return null;
		}

		GetComponent<AudioSource>().Stop();
		GetComponent<AudioSource>().volume = 1;
	}
}
