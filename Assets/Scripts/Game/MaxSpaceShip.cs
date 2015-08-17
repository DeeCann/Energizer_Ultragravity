using UnityEngine;
using System.Collections;

public class MaxSpaceShip : MonoBehaviour {
	
	[SerializeField]
	private int _maxSpaceShip = 3;
	
	void Start () {	
		GameControler.Instance.SpaceshipCounter = _maxSpaceShip;
	}
	
	void Update () {		
		if(_maxSpaceShip < 0)
			GameControler.Instance.LevelFailed();
	}
}