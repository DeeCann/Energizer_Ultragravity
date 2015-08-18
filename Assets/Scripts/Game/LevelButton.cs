using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

	public AudioClip _failTouch;
	public AudioClip _properTouch;

	public GameManager.GlobalLevelsNames LevelName;
	
	private GameObject _activeIco;
	private GameObject _unactiveIco;
	private GameObject _number;

	private bool _isLocked = true;

	void Start () {
		if(PlayerPrefs.HasKey(LevelName.ToString())) {
			_isLocked = false;

			_activeIco = transform.FindChild("ActiveLevel").gameObject;
			_unactiveIco = transform.FindChild("UnactiveLevel").gameObject;
			_number = transform.FindChild("Number").gameObject;

			_activeIco.SetActive(true);
			_unactiveIco.SetActive(false);

			if(LevelName.ToString().Contains("Basic"))
				_number.GetComponent<Text>().color = new Color(0.118f, 0.094f, 0.345f, 1);
		}
	}

	public void PlayLevel() {
		if(!_isLocked) {
			GameControler.Instance.LoadLevel(LevelName.ToString());
			GetComponent<AudioSource>().clip = _properTouch;
			GetComponent<AudioSource>().Play();
		} else {
			GetComponent<AudioSource>().clip = _failTouch;
			GetComponent<AudioSource>().Play();
		}
	}
}
