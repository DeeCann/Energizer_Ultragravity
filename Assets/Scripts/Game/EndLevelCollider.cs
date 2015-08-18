using UnityEngine;
using System.Collections;

public class EndLevelCollider : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent<Collider>().tag == Tags.Player) {
			GameControler.Instance.IsLevelSuccess = true;
			GameControler.Instance.LevelSuccess();
		}

	}
}
