using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour {

	private Vector3 _randomRotation;
	private AudioSource _collectSound;

	void Start () {
		_randomRotation = new Vector3(1,1,1);
		_collectSound = GetComponent<AudioSource>();
	}
	
	void Update () {
		transform.Rotate(_randomRotation);	
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.GetComponent<Collider>().tag == Tags.Player) {
			if(!_collectSound.isPlaying)
				_collectSound.Play();

			foreach(MeshRenderer childRenderer in transform.GetComponentsInChildren<MeshRenderer>())
				childRenderer.enabled = false;

			StartCoroutine(DestroyBeam());
			GameControler.Instance.Points = 1;
			ShipEnergy.Instance.GetEnergy = 100;
		}
	}

	IEnumerator DestroyBeam() {
		yield return new WaitForSeconds(1);
		Destroy(gameObject);
	}
}
