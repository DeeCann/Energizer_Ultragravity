using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Webservice : MonoBehaviour {

	public InputField Code;
	public Text ErrorMsg;
	public Image AlertImage;

	private string _errMessage = "Your code has been already used,\nor is incorrect";
	
	public void Send() {
		if(!IsCodeCorrect) {
			ErrorMsg.text = _errMessage;
			AlertImage.enabled = true;
			Debug.Log(Code.text);
		} else 
			UnlockAllLevelsPanel.Instance.SuccessCodeUnlockPanel();


	}

	public void Test() {
		Debug.Log("connect");

		StartCoroutine(Connect());
	}

	private bool IsCodeCorrect {
		get {
			if(Code.text == "123")
				return true;
			else
				return false;
		}
	}
	IEnumerator Connect()
	{
		// Pull down the JSON from our web-service
		
		WWWForm form = new WWWForm();
		form.AddField("get_message", "11111");
	
		
		WWW w = new WWW("http://localhost/EnergizerBM/server.php/get_message", form);
		
		
		yield return w;
		
		print("Waiting for sphere definitions\n");
		
		// Add a wait to make sure we have the definitions
		
		yield return new WaitForSeconds(1f);
		
		print("Received sphere definitions\n");
		
		// Extract the spheres from our JSON results
		
		//ExtractSpheres(w.text);
		print(w.text);
	}
}
