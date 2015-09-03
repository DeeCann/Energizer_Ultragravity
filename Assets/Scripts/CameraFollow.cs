using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	void FixedUpdate() {
		if(!GameControler.Instance.IsLevelFailed && GameControler.Instance.MyRocket != null) {
			Vector3 followPos = new Vector3();
			followPos = GameControler.Instance.MyRocket.position;
			followPos.x = 12;
			followPos.y = Mathf.Clamp(followPos.y, -15, 15);
			transform.position = followPos;
		}
	}
}
