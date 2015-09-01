using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public enum GlobalLevelsNames
	{
		Basic_1,
		Basic_2,
		Basic_3,
		Basic_4,
		Basic_5,
		Basic_6,
		Basic_7,
		Basic_8,
		Basic_9,
		Basic_10,
		Pack1_11,
		Pack1_12,
		Pack1_13,
		Pack1_14,
		Pack1_15,
		Pack1_16,
		Pack1_17,
		Pack1_18,
		Pack1_19,
		Pack1_20,
		Pack1_21,
		Pack1_22,
		Pack1_23,
		Pack1_24,
		Pack1_25,
		Pack2_26,
		Pack2_27,
		Pack2_28,
		Pack2_29,
		Pack2_30,
		Pack2_31,
		Pack2_32,
		Pack2_33,
		Pack2_34,
		Pack2_35,
		Pack2_36,
		Pack2_37,
		Pack2_38,
		Pack2_39,
		Pack2_40,
		Pack3_41,
		Pack3_42,
		Pack3_43,
		Pack3_44,
		Pack3_45,
		Pack3_46,
		Pack3_47,
		Pack3_48,
		Pack3_49,
		Pack3_50,
		Pack3_51,
		Pack3_52,
		Pack3_53,
		Pack3_54,
		Pack3_55
	}



	private static GameManager _instance = null;
	public static GameManager Instance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
			
			return _instance;
		}
	}

	void Awake() {

		_instance = this;

		if(!PlayerPrefs.HasKey("Basic_1"))
			PlayerPrefs.SetInt("Basic_1", 1);

		Application.targetFrameRate = 60;
	}
}
