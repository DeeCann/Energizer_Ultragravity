using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControler : MonoBehaviour {
	private bool _levelSuccess = false;
	private bool _levelStarted = false;
	private bool _levelFailed = false;
	private bool _shieldIsActive = false;

	private int _spaceShipLeft = 0;
	private int _points = 0;

	private GameObject _spaceShip;

	
	private List<ShieldEnergy> _myCollectedEnergy = new List<ShieldEnergy>();

	private static GameControler _instance = null;
	public static GameControler Instance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType(typeof(GameControler)) as GameControler;
			
			return _instance;
		}
	}


	public class ShieldEnergy {
		private float[] _energyTimes = new float[3];
		private int _currentEnergyPointCounter = 0;
		private int _maxTimePeriod = 3;

		public ShieldEnergy(float time) {
			_energyTimes[_currentEnergyPointCounter] = time;
			_currentEnergyPointCounter++;
		}

		public float AddNewEnergyPoint {
			set {
				_energyTimes[_currentEnergyPointCounter] = value;
				_currentEnergyPointCounter++;
			}
		}

		public bool CanAddNewEnergyPoint {
			get {
				if(_currentEnergyPointCounter >= 3)
					return false;
				else
					return true;
			}
		}

		public bool CanActivateShield {
			get {
				if(_currentEnergyPointCounter == 3) {
					if(_energyTimes[_energyTimes.Length-1] - _energyTimes[0] <= _maxTimePeriod)
						return true;
					else
						return false;
				} else 
					return false;
			}
		}
	}

	void Awake() {
		_levelStarted = false;
		_shieldIsActive = false;
		_instance = this;

		_spaceShip = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Start() {
		FadeScreen.Instance.StartScene();
		
		StartCoroutine(StartLevel());

		_myCollectedEnergy.Clear();

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
		StartCoroutine(ResetSpaceShipAfterTime());
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

	public bool IsShieldActive {
		get {
			return _shieldIsActive;
		}

		set {
			_shieldIsActive = value;
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

	public void ClearCollectedEnergyPoints() {
		_myCollectedEnergy.Clear();
	}

	public int Points {
		set {
			_points += value;

			if(!IsShieldActive) {
				foreach(ShieldEnergy _myEnergyPoints in  _myCollectedEnergy) {
					if(_myEnergyPoints.CanAddNewEnergyPoint)
						_myEnergyPoints.AddNewEnergyPoint = Time.time;
				}

				_myCollectedEnergy.Add(new ShieldEnergy(Time.time));

				foreach(ShieldEnergy _myEnergyPoints in  _myCollectedEnergy) {
					if(_myEnergyPoints.CanActivateShield) {
						_spaceShip.GetComponent<RocketControl>().ActivateShield();
						break;
					}	
				}
			}
		}

		get {
			return _points;
		}
	}

	public Transform MyRocket {
		get {
			if(_spaceShip != null)
				return _spaceShip.transform;
			else
				return null;
		}
	}

	private void CreateNewSpaceShip() {
		_spaceShip = (GameObject)Instantiate(Resources.Load("Rocket"), new Vector3(0,0,0), Quaternion.identity);
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

	
	IEnumerator ResetSpaceShipAfterTime() {
		
		yield return new WaitForSeconds(2);

		InputEventHandler.ResetInput();
		if(SpaceshipCounter > 1)
			CreateNewSpaceShip();
		else
			LevelFailed();
		
		
	}


}
