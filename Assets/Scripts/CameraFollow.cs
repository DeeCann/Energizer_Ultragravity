using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	void FixedUpdate() {
		if(!GameControler.Instance.IsLevelFailed) {
			Vector3 followPos = new Vector3();
		//	followPos = Vector3.Lerp(transform.position, GameControler.Instance.MyRocket.position + GameControler.Instance.MyRocket.transform.forward, Time.deltaTime * 2);
			followPos = GameControler.Instance.MyRocket.position;
			followPos.x = 12;
			transform.position = followPos;
		}
	}
}
