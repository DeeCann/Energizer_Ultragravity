using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipEnergy : MonoBehaviour {

	private float _maxEnergyValue = 620;
	private float _newEnergyLevel;

	private bool _isIncreasingEnergy = false;
	private bool _shipDestroied = false;

	[Range(0,2)]
	public float _shipEnergyFactor = 1f;

	private static ShipEnergy _instance = null;
	public static ShipEnergy Instance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType(typeof(ShipEnergy)) as ShipEnergy;
			
			return _instance;
		}
	}

	void Awake() {
		//if(PlayerPrefs.HasKey("HasCode") && Application.loadedLevel == 12)
			//shipEnergyFactor = 0.1f;
		//Debug.Log(_shipEnergyFactor);
		_instance = this;
		_maxEnergyValue = 620;
		_newEnergyLevel = 0;
		_isIncreasingEnergy = false;
		_shipDestroied = false;
	}

	void Update () {
		if(!GameControler.Instance.MyRocket)
			return; 
		if(GameControler.Instance.IsLevelSuccess)
			return;

		if(!_isIncreasingEnergy ) {
			_maxEnergyValue = _maxEnergyValue - (GameControler.Instance.MyRocket.GetComponent<RocketControl>()._shipEnergyFactor * 2);
			Vector2 _newSize = new Vector2(_maxEnergyValue, GetComponent<RectTransform>().sizeDelta.y);
			GetComponent<RectTransform>().sizeDelta = _newSize;

			if(_maxEnergyValue <= 0 && !_shipDestroied) {
				_shipDestroied = true;
				GameControler.Instance.MyRocket.GetComponent<RocketControl>().DestroyShip();
			}

			Vector2 _newPosition = new Vector2(((620 - _maxEnergyValue) * 17) / 620, GetComponent<RectTransform>().anchorMax.y);
			GetComponent<RectTransform>().anchoredPosition = _newPosition;
		}
	}

	public float GetEnergy {
		set {
			_isIncreasingEnergy = true;
			_newEnergyLevel = _maxEnergyValue + value;
			if(_newEnergyLevel > 620)
				_newEnergyLevel = 620;

			StartCoroutine(IncreaseEnergy());
		}
	}

	public void ResetEnergy() {
		//_instance = this;
		_maxEnergyValue = 620;
		_newEnergyLevel = 0;
		_isIncreasingEnergy = false;
		_shipDestroied = false;
	}

	IEnumerator IncreaseEnergy() {
		while(_maxEnergyValue < _newEnergyLevel) {
			_maxEnergyValue += 2f;

			Vector2 _newSize = new Vector2(_maxEnergyValue, GetComponent<RectTransform>().sizeDelta.y);
			GetComponent<RectTransform>().sizeDelta = _newSize;

			Vector2 _newPosition = new Vector2(((620 - _maxEnergyValue) * 17) / 620, GetComponent<RectTransform>().anchorMax.y);
			GetComponent<RectTransform>().anchoredPosition = _newPosition;


			yield return null;
		}

		_maxEnergyValue = _newEnergyLevel;
		_isIncreasingEnergy = false;
	}
}
