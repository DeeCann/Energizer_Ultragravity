using UnityEngine;
using System.Collections;

public class SFXControler : MonoBehaviour {

	private static SFXControler _instance = null;
	private float _startVolume = 0;

	public static SFXControler Instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType(typeof(SFXControler)) as SFXControler;
			}
			return _instance;
		}
	}
	
	void Awake() {
		if(_instance == null)
			GetComponent<AudioSource>().Play();

		_instance = this;	
		DontDestroyOnLoad(this.gameObject);

		_startVolume = GetComponent<AudioSource>().volume;
	}

	public void VolumeDown() {
		Debug.Log("vol dowN");
		StartCoroutine(GetVolumeDown());
	}

	public void VolumeUp() {
		StartCoroutine(GetVolumeUp());
	}

	IEnumerator GetVolumeDown() {
		while (GetComponent<AudioSource>().volume > 0.1f) {
			GetComponent<AudioSource>().volume -= 0.2f;
			yield return 0;
		}
	}

	IEnumerator GetVolumeUp() {
		while (GetComponent<AudioSource>().volume < _startVolume) {
			GetComponent<AudioSource>().volume += 0.2f;
			yield return 0;
		}
	}
}
