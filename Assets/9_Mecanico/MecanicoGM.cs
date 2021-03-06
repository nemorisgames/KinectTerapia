﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MecanicoGM : MonoBehaviour
{
	public static MecanicoGM Instance;
	public MecanicoMano mano;
	public Transform moveRef;
	public bool moveWithMouse = false;
	public float height, width;
	public enum PartType
	{
		None,
		Part1,
		Part2,
		Part3,
		Part4,
		Part5
	}
	public GameObject partPrefab;
	public GameObject[] slotsObjects;
	private MecanicoSlot[] partSlots;
	public Dificultad.Nivel dificultad;
	public int numParts = 5;
	public int numSlots = 3;
	private float stageTime;
	public float time;
	private float savedTime;
	public UILabel timeLabel;
	public int correct = 0;
	private int score;
	List<MecanicoPieza> spawnedParts = new List<MecanicoPieza> ();
	private PositionTransformator positionTransformator;
	public float randomDistance = 0.75f;

	void Awake ()
	{
		if (Instance == null)
			Instance = this;
		Random.InitState (System.DateTime.Now.Second * System.DateTime.Now.Minute);
		positionTransformator = GameObject.FindObjectOfType<PositionTransformator> ();
	}

	void Start ()
	{

	}

	void EnableSlots (int slot)
	{
		for (int i = 0; i < slotsObjects.Length; i++)
		{
			slotsObjects[i].SetActive (false);
		}
		slotsObjects[slot].SetActive (true);
	}

	public void Init ()
	{
		int index = (int) dificultad;
		correct = 0;
		time = 0;
		switch (dificultad)
		{
			case Dificultad.Nivel.facil:
				stageTime = 5;
				numSlots = 2;
				numParts = 3;
				score = 200;
				stageTime = 5;
				break;
			case Dificultad.Nivel.medio:
				stageTime = 4;
				numSlots = 3;
				numParts = 5;
				score = 400;
				stageTime = 4;
				break;
			case Dificultad.Nivel.dificil:
				stageTime = 3;
				numSlots = 5;
				numParts = 10;
				score = 500;
				stageTime = 3;
				break;
		}
		SetTimeLabel (stageTime * 60);
		MecanicoPieza[] parts = GameObject.FindObjectsOfType<MecanicoPieza> ();
		for (int i = 0; i < parts.Length; i++)
		{
			Destroy (parts[i].gameObject);
		}
		//partSlots = new MecanicoSlot[numSlots];
		EnableSlots (index);

		/* for(int i = 0; i < partSlots.Length; i++){
			partSlots[i].type = (PartType)(i+1);
		}*/
		spawnedParts.Clear ();
		for (int i = 0; i < numSlots; i++)
		{
			GameObject g = (GameObject) Instantiate (partPrefab, partPrefab.transform.position, partPrefab.transform.rotation);
			MecanicoPieza p = g.GetComponent<MecanicoPieza> ();
			spawnedParts.Add (p);

			Vector3 randPos = randomPosition ();
			int iter = 0;
			while (iter < 20 && !CheckPos (randPos, randomDistance)){
				randPos = randomPosition ();
				iter++;
			}
			g.transform.position = randPos;
			if (i < numSlots)
				p.type = (PartType) (i + 1);
			else
				p.type = PartType.None;
			p.initPos = g.transform.position;
		}
	}

	Vector3 randomPosition ()
	{
		Vector3 r = new Vector3 ();
		float rx_left = Random.Range(Mathf.Max(positionTransformator.horLimits.x,-4f),-1.6f);
		float rx_right = Random.Range(1.6f,Mathf.Min(positionTransformator.horLimits.y,4f));
		r.x = (Random.Range(0,2) == 1 ? rx_left : rx_right);
		if(Mathf.Abs(r.x) < 1.6f)
			r.x = 1.6f * Mathf.Sign(r.x);

		//float ry_down = Random.Range(Mathf.Min(Mathf.Max(positionTransformator.horLimits.x,-4f),-2.1f),-2f);
		float ry_up = Random.Range(0,Mathf.Min(positionTransformator.horLimits.y,4f));
		//r.y = (Random.Range(0,2) == 1 ? ry_down : ry_up);
		r.y = ry_up;

		r.z = Random.Range (-1.8f, 0.2f);
		return r;
	}

	bool CheckPos (Vector3 newPos, float margen)
	{
		foreach (MecanicoPieza p in spawnedParts)
		{
			float distance = Vector3.Distance (newPos, p.transform.position);
			if (distance < margen)
				return false;
		}
		return true;
	}

	void Update ()
	{
		if (Time.timeScale == 0)
			return;

		if (moveWithMouse)
		{
			/*
			Vector3 handPosition = Input.mousePosition - new Vector3(Screen.width/2, Screen.height/2, 0f);
			Vector3 viewport = Camera.main.ScreenToViewportPoint(handPosition);
			float vPos = viewport.y * height;
			float hPos = viewport.x * width;
			float zPos = Mathf.Clamp(moveRef.position.z + Input.GetAxis("Vertical") * Time.deltaTime * 5, -3,8);
			moveRef.position = new Vector3(hPos,vPos,zPos);
			*/
			int vMove = 0;
			if (Input.GetKey (KeyCode.Q))
				vMove = 1;
			else if (Input.GetKey (KeyCode.E))
				vMove = -1;
			float vPos = Mathf.Clamp (moveRef.position.y + vMove * Time.deltaTime * 5, -3, 8);
			float hPos = Mathf.Clamp (moveRef.position.x + Input.GetAxis ("Horizontal") * Time.deltaTime * 5, -8, 8);
			float zPos = Mathf.Clamp (moveRef.position.z + Input.GetAxis ("Vertical") * Time.deltaTime * 5, -8, 8);
			moveRef.position = new Vector3 (hPos, vPos, zPos);
		}

	}

	void LateUpdate ()
	{
		if (Time.timeScale == 0)
			return;
		time = stageTime * 60 - (Time.time - savedTime);
		SetTimeLabel (time);
	}

	void SetTimeLabel (float f)
	{
		timeLabel.text = Mathf.FloorToInt (f / 60) + ":" + (Mathf.FloorToInt (f % 60) < 10 ? "0" : "") + (Mathf.FloorToInt (f % 60));
	}

	public void CorrectPart ()
	{
		correct++;
		GameManager.instance.AddToScore (score);
		if (correct >= numSlots)
		{
			savedTime = Time.time;
			GameManager.instance.FinishLevel ();
			switch (dificultad)
			{
				case Dificultad.Nivel.facil:
					dificultad = Dificultad.Nivel.medio;
					break;
				case Dificultad.Nivel.medio:
					dificultad = Dificultad.Nivel.dificil;
					break;
			}
		}
	}
}