using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPosition : MonoBehaviour {
	public static HandPosition Instance;
	List<Vector2> savedPos = new List<Vector2>();
	List<Vector4> points = new List<Vector4>();
	public float tiempoMuestreo = 1f;

	// Use this for initialization
	void Awake () {
		if(Instance == null)
			Instance = this;
	}

	void Start(){
		StartCoroutine(NextPosition());
	}

	IEnumerator NextPosition(){
		yield return new WaitForSeconds(tiempoMuestreo);
		Vector4 position = new Vector4(transform.position.x,transform.position.y,transform.position.z,0);
		position.x = (Mathf.Round(position.x * 10))/10f;
		position.y = (Mathf.Round(position.y * 10))/10f;
		position.z = (Mathf.Round(position.z * 10))/10f;
		Debug.Log("saved "+position.x+","+position.y+","+position.z);
		points.Add(position);
		//if(Heatmap.Instance != null)
		//	Heatmap.Instance.AddPoint(position);
		if(Time.timeScale != 0)
			StartCoroutine(NextPosition());
	}

	public void SavePoints(){
		Heatmap.Instance.Init(points);
	}
}
