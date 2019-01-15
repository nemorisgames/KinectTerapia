using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBar : MonoBehaviour {
    public float widthSensitivity = 5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if(Time.timeScale == 0f) return;
        Vector3 handPosition = Input.mousePosition - new Vector3(Screen.width / 2f, 0f, 0f);
        float horizontalViewportPosition = Camera.main.ScreenToViewportPoint(handPosition).x;
        float horizontalPosition = horizontalViewportPosition * widthSensitivity;
        transform.position = new Vector3(horizontalPosition, 0f, 0f);
	}
}
