using UnityEngine;
using System.Collections;

public class NextScene : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if(other.name == "Sphere") {
			if(Application.loadedLevel == 3)
				GameControler.Instance.LoadNextLevel();
			else
				GameControler.Instance.LoadNextLevel();
		}
			
	}
}
