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
		if(!PlayerPrefs.HasKey("LevelPacksUnlocked")) {
			PlayerPrefs.SetInt("CanUnlock", 1);
			GameControler.Instance.LoadLevel("UnlockAllLevels");
		} else {
			GameControler.Instance.LoadLevel("Start");
		}
	}
}
