using UnityEngine;
using System.Collections;

public class CrashCotroler : MonoBehaviour {
	
	void Start () {
		GetComponent<Animation>().Stop();
		// By default loop all animations
		GetComponent<Animation>().wrapMode = WrapMode.Once;

		AnimationState Crash = GetComponent<Animation>()["Crash"];
		Crash.wrapMode = WrapMode.Once;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
