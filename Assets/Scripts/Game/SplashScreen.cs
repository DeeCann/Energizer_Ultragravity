using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {
	
	void Start () {
		StartCoroutine(WaitForLoadStart());
	}

	IEnumerator WaitForLoadStart() {
		yield return new WaitForSeconds(5);

		GameControler.Instance.LoadLevel("Start");
	}

}
