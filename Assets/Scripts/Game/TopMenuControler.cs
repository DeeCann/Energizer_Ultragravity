using UnityEngine;
using System.Collections;

public class TopMenuControler : MonoBehaviour {

	public void Reload() {
		GameControler.Instance.ReloadLevel();
	}

	public void Back() {
		GameControler.Instance.LoadLevel("Start");
	}
}
