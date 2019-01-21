using UnityEngine;
using System.Collections;

public class RandomColor : MonoBehaviour {
    MeshRenderer meshRenderer;
	public Color[] colors;

    //From 0 to 100, if 0, always visible 
    [Range(0.0f, 100.0f)]
    public int invisibleProbability = 30;
	void Start () {
        meshRenderer = GetComponent<MeshRenderer>();
        Generate();
	}

    //Generate a new random color or hide the object
	public void Generate(){
		if (invisibleProbability > 0 && Random.Range (0, 100) < invisibleProbability) {
            AssignColor(Color.clear);
			return;
		}
		int colorSelected = Random.Range (0, colors.Length);
		if (colors.Length > 0) {
            AssignColor(colors[colorSelected]);
		}
	}

    //assign the color to the correct component
    void AssignColor(Color color)
    {
        meshRenderer.material.color = color;
    }
	
	void Update () {
	
	}
}
