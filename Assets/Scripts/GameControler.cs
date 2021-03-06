﻿using UnityEngine;
using System.Collections;

public class GameControler : MonoBehaviour {
	private bool _levelSuccess = false;
	private bool _levelStarted = false;
	private bool _levelFailed = false;
	
	private int _spaceShipLeft = 0;
	private int _points = 0;

	private GameObject _spaceShip;

	private static GameControler _instance = null;
	public static GameControler Instance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType(typeof(GameControler)) as GameControler;
			
			return _instance;
		}
	}
	
	void Awake() {
		_levelStarted = false;
		_instance = this;

		_spaceShip = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Start() {
		FadeScreen.Instance.StartScene();
		
		StartCoroutine(StartLevel());

//		if(SFXControler.Instance != null)
//			SFXControler.Instance.VolumeUp();
	}
	
	public void ClearAllPrefs() {
		PlayerPrefs.DeleteAll();
		LoadLevel("Start");
	}
	
	public void LoadLevel(string level) {
		FadeScreen.Instance.EndScene(level);
	}
	
	public void LoadNextLevel() {
		PlayerPrefs.DeleteKey("LastMoleculeSelected");
		
		if(Application.loadedLevel == 12)
			FadeScreen.Instance.EndScene(null, 1);
		else
			FadeScreen.Instance.EndScene(null, Application.loadedLevel+1);
	}
	
	public void ReloadLevel(int reloadAfterTime = 0) {
		StartCoroutine(ReloadAfterTime(reloadAfterTime));
	}
	
	public void LevelSuccess() {
		string nextLevelName = Application.loadedLevelName.Substring(0,6)+(System.Convert.ToInt16( Application.loadedLevelName.Substring(6))+1);
		if(Application.CanStreamedLevelBeLoaded(nextLevelName))
			PlayerPrefs.SetInt(Application.loadedLevelName.Substring(0,6)+(System.Convert.ToInt16( Application.loadedLevelName.Substring(6))+1), 1);
		
		if(System.Convert.ToInt16( Application.loadedLevelName.Substring(6)) == 10)
			LevelsComplete.Instance.LevelsCompleted();
		else
			LoadNextLevel();
	}
	
	public void LevelFailed() {
		IsLevelFailed = true;
		ReloadLevel();
	}

	public void ResetSpaceShip() {
		InputEventHandler.ResetInput();
		if(SpaceshipCounter > 1)
			CreateNewSpaceShip();
		else
			LevelFailed();
	}
	
	public bool IsLevelSuccess {
		get {
			return _levelSuccess;
		}
		
		set {
			_levelSuccess = true;
		}
	}
	
	public bool IsLevelStarted {
		get {
			return _levelStarted;
		}
	}

	public bool IsLevelFailed {
		get {
			return _levelFailed;
		}
		
		set {
			_levelFailed = true;
		}
	}
	
	public int SpaceshipCounter {
		set {
			_spaceShipLeft = value;
		}
		
		get {
			return _spaceShipLeft;
		}
	}

	public int Points {
		set {
			_points += value;
		}

		get {
			return _points;
		}
	}

	public Transform MyRocket {
		get {
			return _spaceShip.transform;
		}
	}

	private void CreateNewSpaceShip() {
		_spaceShip = (GameObject)Instantiate(Resources.Load("Rocket"), Vector3.zero, Quaternion.identity);
		_spaceShip.GetComponent<Rigidbody>().velocity = Vector3.zero;	
		SpaceshipCounter = --SpaceshipCounter;

		ShipEnergy.Instance.ResetEnergy();
	}
	
	IEnumerator StartLevel() {
		yield return new WaitForSeconds(0.5f);
		
		_levelStarted = true;
	}
	
	IEnumerator ReloadAfterTime(int time) {
		yield return new WaitForSeconds(time);
		
		FadeScreen.Instance.EndScene(null, Application.loadedLevel);
	}
	
	IEnumerator LoadNextLevelWithTimer(int _timer) {
		yield return new WaitForSeconds(_timer);
		//SFXControler.Instance.VolumeUp();
		LoadNextLevel();
	}
}
