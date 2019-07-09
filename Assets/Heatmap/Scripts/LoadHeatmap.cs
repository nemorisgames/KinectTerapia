using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadHeatmap : MonoBehaviour
{
	private UITexture image;
	public Texture defaultImage;

	void Awake ()
	{
		image = GetComponentInChildren<UITexture> ();
	}

	public void LoadImage (string imgPath)
	{
		if (imgPath == "")
		{
			LoadDefault ();
			return;
		}
		string _path = Path.Combine (Application.persistentDataPath, imgPath);
		if (System.IO.File.Exists (_path))
		{
			byte[] bytes = System.IO.File.ReadAllBytes (_path);
			Texture2D texture = new Texture2D (1, 1);
			texture.LoadImage (bytes);
			image.mainTexture = texture;
		}
		else
		{
			LoadDefault ();
		}
	}

	void LoadDefault ()
	{
		image.mainTexture = defaultImage;
	}
}