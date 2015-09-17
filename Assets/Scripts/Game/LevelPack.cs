using UnityEngine;
using System.Collections;

public class LevelPack : MonoBehaviour {

	public Transform LevelStandardPanel;
	public Transform Back;

	public void OpenPanel() {
		if(PlayerPrefs.HasKey("LevelPacksUnlocked")) {
			LevelStandardPanel.GetComponent<Animator>().SetBool("FadeIn", false);
			LevelStandardPanel.GetComponent<Animator>().SetBool("FadeOut", true);

			GetComponent<Animator>().SetBool("FadeOut", false);
			GetComponent<Animator>().SetBool("FadeIn", true);

			GetComponent<AudioSource>().Play();
		}
//		else {
//			GameControler.Instance.LoadLevel("UnlockAllLevels");
//		}
	}

	public void ClosePanel() {
		GetComponent<Animator>().SetBool("FadeIn", false);
		GetComponent<Animator>().SetBool("FadeOut", true);

		LevelStandardPanel.GetComponent<Animator>().SetBool("FadeOut", false);
		LevelStandardPanel.GetComponent<Animator>().SetBool("FadeIn", true);
	}
}
