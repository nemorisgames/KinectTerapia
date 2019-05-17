using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class LoadHeatmap : MonoBehaviour {
	private RawImage image;
	public Transform [] ejeX, ejeY, ejeZ;
	public LineRenderer lrX, lrY, lrZ;

	void Awake(){
		image = GetComponentInChildren<RawImage>();
	}

	void Start () {
		//PlayerPrefs.SetString("currentHeatmap","Heatmap/0_9_8-4-2019_17-39.png");
		LoadImage(PlayerPrefs.GetString("currentHeatmap"));
		lrX.SetPositions(new Vector3[]{ejeX[0].position,ejeX[1].position});
		lrY.SetPositions(new Vector3[]{ejeY[0].position,ejeY[1].position});
		lrZ.SetPositions(new Vector3[]{ejeZ[0].position,ejeZ[1].position});
	}

	void LoadImage(string imgPath){
		if(imgPath == "")
			return;
		string _path = Path.Combine(Application.persistentDataPath,imgPath);
		if(System.IO.File.Exists(_path)){
			byte[] bytes = System.IO.File.ReadAllBytes(_path);
			Texture2D texture = new Texture2D(1, 1);
			texture.LoadImage(bytes);
			image.texture = texture;
		}
	}
}
