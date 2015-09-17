using UnityEngine;
using System.Collections;

public class EndLevelCollider : MonoBehaviour {

	void Start() {
		if(!PlayerPrefs.HasKey("HasCode") && Application.loadedLevel == 12)
			gameObject.SetActive(false);
	}

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent<Collider>().tag == Tags.Player) {
			GameControler.Instance.IsLevelSuccess = true;
			GameControler.Instance.LevelSuccess();
		}

	}
}
