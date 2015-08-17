using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour {

	private Vector3 _randomRotation;
	
	void Start () {
		_randomRotation = new Vector3(1,1,1);
	}
	
	void Update () {
		transform.Rotate(_randomRotation);	
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.GetComponent<Collider>().tag == Tags.Player) {
			Destroy(gameObject);
			GameControler.Instance.Points = 1;
		}
	}
}
