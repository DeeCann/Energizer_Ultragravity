using UnityEngine;
using System.Collections;

public class TopMenuControler : MonoBehaviour {

	public void Reload() {
		GameControler.Instance.ReloadLevel();
	}

	public void Back() {
		Application.LoadLevel(1);
	}
}
