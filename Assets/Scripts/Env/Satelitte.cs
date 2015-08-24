using UnityEngine;
using System.Collections;

public class Satelitte : MonoBehaviour {

	private Vector3 _randomRotation;
	
	private ParticleSystem _explosion;
	
	void Start () {
		_randomRotation = new Vector3(Random.Range(-2,2),Random.Range(-2,2),Random.Range(-2,2));
		_explosion = transform.FindChild("ExplotionParticles").GetComponent<ParticleSystem>();
	}
	
	void Update () {
		transform.Rotate(_randomRotation);	
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.GetComponent<Collider>().tag == Tags.Player) {
			_explosion.transform.parent = null;
			_explosion.Emit(100);
			
			if(!GetComponent<AudioSource>().isPlaying)
				GetComponent<AudioSource>().Play();
			
			foreach(MeshRenderer child in gameObject.GetComponentsInChildren<MeshRenderer>())
				child.enabled = false;
			
			Destroy(gameObject, 1);
		}
	}
}
