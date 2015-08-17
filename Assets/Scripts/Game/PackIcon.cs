using UnityEngine;
using System.Collections;

public class PackIcon : MonoBehaviour {
	
	void Start () {
		if(PlayerPrefs.HasKey("LevelPacksUnlocked"))
			transform.FindChild("Locker").gameObject.SetActive(false);
	}
}
