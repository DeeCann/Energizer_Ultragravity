using UnityEngine;
using System.Collections;

public class Wormhole : MonoBehaviour {

	[SerializeField]
	private Transform _destination;
	
	public Transform Destination {
		get {
			return _destination;
		}
	}
}
