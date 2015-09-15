using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Webservice : MonoBehaviour {
	public Transform Splash;

	public InputField Code;
	public Text ErrorMsg;
	public Image AlertImage;

	private string[] _errMessage = new string[]{ "Your code has been already used", "Your code is incorrect", "Code is empty" };

	private JSONObject _webserwisResponse; 
	private string jsonBoolResponse;
	private string jsonCodeResponse;

	private bool _connectRequest = false;
	private bool _connecting = false;
	private bool _receiving = false;

	void Update() {
		if(!_connectRequest)
			return;

		if(_connecting)
			return;

		if(_receiving) {
			if(!IsCodeCorrect) {
				AlertImage.enabled = true;
				ErrorMsg.enabled = true;
			} else {
				PlayerPrefs.SetInt("HasCode", 1);
				PlayerPrefs.SetInt("UnlockMaxBounce", 2);
				GameControler.Instance.ReloadLevel();
				Debug.Log("ok");
			}

			_receiving = false;
			_connectRequest = false;
		}
	}


	public void Send() {
		Debug.Log(Code.text.Length);
		if(Code.text.Length == 0) {
			ErrorMsg.text = _errMessage[2];
			ErrorMsg.enabled = true;
			AlertImage.enabled = true;
			return;
		} 

		_connectRequest = true;
		Splash.gameObject.SetActive(true);
		StartCoroutine(Connect());
	}

	private bool IsCodeCorrect {
		get {
			if(jsonBoolResponse == "true")
				return true;
			else {
				if(jsonCodeResponse == "201")
					ErrorMsg.text = _errMessage[1];
				else
					ErrorMsg.text = _errMessage[0];
				return false;
			}
		}
	}

	IEnumerator Connect()
	{
		_connecting = true;

		WWWForm form = new WWWForm();
		form.AddField("get_message", "tmp");
		WWW request = new WWW("http://apps.pc-fb.com/energizer-100-games/public/auth/code2?code="+Code.text, form);
		yield return request;
		
		print("Waiting ...");	
		
		yield return new WaitForSeconds(1f);
		
		print("Received ...");

		_webserwisResponse = new JSONObject(request.text);

		Debug.Log(_webserwisResponse);
		foreach(JSONObject item in _webserwisResponse.list)
		{
			if (item.type == JSONObject.Type.BOOL)
				jsonBoolResponse = item.ToString();

			if (item.type == JSONObject.Type.NUMBER)
				jsonCodeResponse = item.ToString();
		}

		Splash.gameObject.SetActive(false);
		_connecting = false;
		_receiving = true;
	}
}
