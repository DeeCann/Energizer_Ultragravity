using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShipEnergy : MonoBehaviour {

	private float _maxEnergyValue = 620;
	private float _newEnergyLevel;

	private bool _isIncreasingEnergy = false;

	private static ShipEnergy _instance = null;
	public static ShipEnergy Instance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType(typeof(ShipEnergy)) as ShipEnergy;
			
			return _instance;
		}
	}

	void Awake() {
		_instance = this;
		_maxEnergyValue = 620;
		_newEnergyLevel = 0;
		_isIncreasingEnergy = false;
	}

	void Update () {
//		if(InputEventHandler._isStartTouchAction && !_isIncreasingEnergy) {
//			Vector2 _newSize = new Vector2(--_maxEnergyValue, GetComponent<RectTransform>().sizeDelta.y);
//			GetComponent<RectTransform>().sizeDelta = _newSize;
//
//			if(_maxEnergyValue <= 0)
//				GameObject.FindGameObjectWithTag("Player").GetComponent<RocketControl>().DestroyShip();
//
//			Vector2 _newPosition = new Vector2(((620 - _maxEnergyValue) * 17) / 620, GetComponent<RectTransform>().anchorMax.y);
//			GetComponent<RectTransform>().anchoredPosition = _newPosition;
//		}
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
		_instance = this;
		_maxEnergyValue = 620;
		_newEnergyLevel = 0;
		_isIncreasingEnergy = false;
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
