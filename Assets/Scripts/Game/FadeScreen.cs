using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour {

	private float fadeSpeed = 4f; 
	private bool _breakFadeInCorutine = false; 
	private Image fadeImage;

	private static FadeScreen _instance = null;
	public static FadeScreen Instance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType(typeof(FadeScreen)) as FadeScreen;
			
			return _instance;
		}
	}

	void Awake ()
	{
		_instance = this;

		fadeImage = GetComponent<Image>();
		fadeImage.color = new Color(0,0,0,1);
	}
	
	public void StartScene()
	{
		StartCoroutine( FadeToClear() );
	}

	public void EndScene (string _sceneName = null, int _sceneId = -1)
	{
		_breakFadeInCorutine = true;
		StartCoroutine( FadeToBlack(_sceneName, _sceneId) );
	}

	IEnumerator FadeToClear() {
		while(fadeImage.color.a > 0.02f) {
			fadeImage.color = Color.Lerp(fadeImage.color, Color.clear, fadeSpeed * Time.deltaTime);

			if(_breakFadeInCorutine)
				yield break;
			else
				yield return 0;
		}

		fadeImage.color = Color.clear; 
	}

	IEnumerator FadeToBlack(string _sceneToLoadName = null, int _sceneToLoadId = -1) {
		while(fadeImage.color.a < 0.98f) {
			fadeImage.color = Color.Lerp(fadeImage.color, Color.black, fadeSpeed * Time.deltaTime);
			yield return 0;
		}
		
		fadeImage.color = Color.black; 

		if(_sceneToLoadName != null) {
			if(Application.CanStreamedLevelBeLoaded(_sceneToLoadName))
				Application.LoadLevel(_sceneToLoadName);
			else
				Application.LoadLevel(0);

			yield break;
		}

		if(_sceneToLoadId != -1) {
			if(Application.CanStreamedLevelBeLoaded(_sceneToLoadId))
				Application.LoadLevel(_sceneToLoadId);
			else
				Application.LoadLevel(0);
			
			yield break;
		}
	}
}
