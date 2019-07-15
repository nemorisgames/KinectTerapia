using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotoObstaculos : MonoBehaviour
{
	public float distanciaInicial = 10;
	public float distanciaObstaculos = 15;
	private PilotoObstaculo[] obstaculos;
	void Start ()
	{
		obstaculos = GetComponentsInChildren<PilotoObstaculo> ();
		for (int i = 0; i < obstaculos.Length; i++)
		{
			obstaculos[i].transform.localPosition = new Vector3 (distanciaInicial + ((i + 1) * distanciaObstaculos), obstaculos[i].transform.localPosition.y, obstaculos[i].transform.localPosition.z);
		}
	}
}