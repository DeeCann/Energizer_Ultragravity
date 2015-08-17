using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BottomMenuControler : MonoBehaviour {
	[SerializeField]
	private Text _SpaceShipCounter;

	[SerializeField]
	private Text _Points;

	private static BottomMenuControler _instance = null;
	public static BottomMenuControler Instance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType(typeof(BottomMenuControler)) as BottomMenuControler;
			
			return _instance;
		}
	}

	void Awake() {
		_instance = this;
	}

	void Update() {
		_SpaceShipCounter.text = GameControler.Instance.SpaceshipCounter.ToString();
		_Points.text = GameControler.Instance.Points.ToString();
	}
}
