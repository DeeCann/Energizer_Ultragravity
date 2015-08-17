using UnityEngine;
using System.Collections;

public class LevelsComplete : MonoBehaviour {

	private static LevelsComplete _instance = null;
	public static LevelsComplete Instance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType(typeof(LevelsComplete)) as LevelsComplete;
			
			return _instance;
		}
	}
	
	void Awake () {
		_instance = this;
	}
	
	public void LevelsCompleted() {
		GetComponent<Animator>().SetBool("Play", true);
		GetComponent<AudioSource>().Play();
	}

	public void UnlockPanel() {
		GameControler.Instance.LoadLevel("UnlockAllLevels");
	}
}
