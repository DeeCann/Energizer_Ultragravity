using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

public class UnlockAllLevelsPanel : MonoBehaviour {

	public Transform _mainUnlockLevelsPanel;
	public Transform _confirmationBuyPanel;
	public Transform _codePanel;
	public Transform _successCodeUnlockPanel;
	public Transform _successBuyUnlockPanel;

	private static UnlockAllLevelsPanel _instance = null;
	public static UnlockAllLevelsPanel Instance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType(typeof(UnlockAllLevelsPanel)) as UnlockAllLevelsPanel;
			
			return _instance;
		}
	}

	public static List<VirtualGood> LifeVGItems = null;

	void Awake() {
		_instance = this;
	}

	void Start() {
		StoreEvents.OnSoomlaStoreInitialized += OnSoomlaStoreInitialized;
		SoomlaStore.Initialize(new BuyPacksAsset());
	}


	public void BuyNewLevels() {
		StartCoroutine(FadeOutCanvas(_mainUnlockLevelsPanel.GetComponent<CanvasGroup>()));
		StartCoroutine(FadeInCanvas(_confirmationBuyPanel.GetComponent<CanvasGroup>()));
	}

	public void CodePanel() {
		StartCoroutine(FadeOutCanvas(_mainUnlockLevelsPanel.GetComponent<CanvasGroup>()));
		StartCoroutine(FadeInCanvas(_codePanel.GetComponent<CanvasGroup>()));
	}

	public void CodePanelFromBuyConfirmation() {
		StartCoroutine(FadeOutCanvas(_confirmationBuyPanel.GetComponent<CanvasGroup>()));
		StartCoroutine(FadeInCanvas(_codePanel.GetComponent<CanvasGroup>()));
	}

	public void SuccessCodeUnlockPanel() {
		StartCoroutine(FadeOutCanvas(_codePanel.GetComponent<CanvasGroup>()));
		StartCoroutine(FadeInCanvas(_successCodeUnlockPanel.GetComponent<CanvasGroup>()));

		PlayerPrefs.SetInt("LevelPacksUnlocked", 1);
		PlayerPrefs.SetInt("Pack1_11", 1);
		PlayerPrefs.SetInt("Pack2_26", 1);
	}

	public void InitializeBuyItem() {
		VirtualGood Premium_Packs = LifeVGItems[0];

		try {
			StoreInventory.BuyItem(Premium_Packs.ItemId);
		} catch(UnityException e) {
			Debug.Log("SOOMLA/UNITY" + e.Message);
		}
	}

	public void SuccessBuyUnlockPanel() {
		StartCoroutine(FadeOutCanvas(_confirmationBuyPanel.GetComponent<CanvasGroup>()));
		StartCoroutine(FadeInCanvas(_successBuyUnlockPanel.GetComponent<CanvasGroup>()));

		PlayerPrefs.SetInt("LevelPacksUnlocked", 1);
		PlayerPrefs.SetInt("Pack1_11", 1);
		PlayerPrefs.SetInt("Pack2_26", 1);
	}


	IEnumerator FadeOutCanvas(CanvasGroup _canvas) {
		while(_canvas.alpha > 0.05f) {
			_canvas.alpha = Mathf.Lerp(_canvas.alpha, 0, Time.deltaTime * 8);
			yield return 0;
		}
		_canvas.alpha = 0;
		_canvas.blocksRaycasts = false;
	}

	IEnumerator FadeInCanvas(CanvasGroup _canvas) {
		while(_canvas.alpha < 0.95f) {
			_canvas.alpha = Mathf.Lerp(_canvas.alpha, 1, Time.deltaTime * 8);
			yield return 0;
		}
		_canvas.alpha = 1;
		_canvas.blocksRaycasts = true;
	}







	public void OnSoomlaStoreInitialized() {
		LifeVGItems = StoreInfo.Goods;
	}

}
