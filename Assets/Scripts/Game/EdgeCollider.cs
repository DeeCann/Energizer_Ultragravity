using UnityEngine;
using System.Collections;

public class EdgeCollider : MonoBehaviour {

	public bool _isLeft;
	public bool _isRight;
	// Use this for initialization
	void Start () {
		SetCollider();
	}
	
	// Update is called once per frame
	void Update () {
		SetCollider();
	}

	private void SetCollider() {
		if(_isLeft)
			transform.position = new Vector3((Camera.main.orthographicSize * Screen.width/Screen.height)+5, 0,0);
		if(_isRight)
			transform.position = new Vector3(((Camera.main.orthographicSize * Screen.width/Screen.height)+5)*-1, 0,0);
	}
}
