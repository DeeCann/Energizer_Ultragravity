using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InputEventHandler : MonoBehaviour {
	public static bool _isStartTouchAction = false;
	public static bool _isEndTouchAction = false;
	public static Vector3 _currentTouchPosition = Vector2.zero;

	private bool _UIHit = false;

	void Update () {
		if(Input.GetMouseButtonDown(0)) {
			
//			if(EventSystem.current.IsPointerOverGameObject(0) || EventSystem.current.IsPointerOverGameObject(1)) {
//				_UIHit = true;
//				
//				return;
//			}
//			
//			if((Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor) && EventSystem.current.IsPointerOverGameObject(-1)) {
//				_UIHit = true;
//				return;
//			}

			_isStartTouchAction = true;
		}

		if(_isStartTouchAction) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			
			if (Physics.Raycast (ray, out hit, Mathf.Infinity))
			{
				_UIHit = false;	
				_currentTouchPosition = new Vector3(hit.point.x, hit.point.y, 0);
			}
			_currentTouchPosition.z = 0;
		}

		if(Input.GetMouseButtonUp(0))
			_isStartTouchAction = false;
	}
}
