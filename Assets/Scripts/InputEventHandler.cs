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
//			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//			RaycastHit hit;
//			
//			if (Physics.Raycast (ray, out hit, Mathf.Infinity))
//			{
//				_UIHit = false;	
//				_currentTouchPosition = new Vector3(0, hit.point.y, hit.point.z);
//			}
//			_currentTouchPosition.x = 0;
//			_currentTouchPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
//			_currentTouchPosition.x = 0;
//			Debug.Log(_currentTouchPosition);




			Plane plane=new Plane(Vector3.right, Vector3.zero);
			Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
			float distance;
			if(plane.Raycast(ray, out distance)) {
				_currentTouchPosition=ray.GetPoint(distance);
				//Instantiate (obj, point, Quaternion.identity);
			}

		}

		if(Input.GetMouseButtonUp(0))
			_isStartTouchAction = false;
	}

	public static void ResetInput() {
		_isStartTouchAction = false;
	}
}
