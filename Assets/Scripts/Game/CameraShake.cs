using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	private Vector3 originPosition = Vector3.zero;
	private Quaternion originRotation = Quaternion.identity;

	private float _shake_decay;
	private float _shake_intensity;
	private float _shake_force = 0.2f;

	private static CameraShake _instance = null;
	public static CameraShake Instance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType(typeof(CameraShake)) as CameraShake;
			
			return _instance;
		}
	}
	
	void Awake () {
		_instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (_shake_intensity > 0 && Time.timeScale > 0)
		{
			transform.position = Vector3.Lerp (transform.position, originPosition + Random.insideUnitSphere * _shake_intensity, Time.deltaTime*50f);
			transform.rotation = Quaternion.Lerp (transform.rotation, new Quaternion(
				originRotation.x + Random.Range(-_shake_intensity, _shake_intensity) * _shake_force,
				originRotation.y + Random.Range(-_shake_intensity, _shake_intensity) * _shake_force,
				originRotation.z + Random.Range(-_shake_intensity, _shake_intensity) * _shake_force,
				originRotation.w + Random.Range(-_shake_intensity, _shake_intensity) * _shake_force), Time.deltaTime*50f);

			_shake_intensity -= _shake_decay;
		}
	}

	public void DoShake() {
		_shake_force = 0.2f;
		_shake_intensity = 0.015f;
		_shake_decay = 0.0008f;

		originPosition = transform.position;
		originRotation = transform.rotation;
	}
}
