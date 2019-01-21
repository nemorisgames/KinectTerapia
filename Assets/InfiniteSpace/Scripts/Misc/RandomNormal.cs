using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class RandomNormal : MonoBehaviour
{
    MeshRenderer meshRenderer;
    //Normal textures that will be used randomly in this spriteRenderer
    public Texture[] textures;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Generate();
    }

    //Generate and assign one of the bump maps randomly
    public void Generate()
    {
        if (textures.Length > 0)
        {
            meshRenderer.material.SetTexture("_BumpMap", textures[Random.Range(0, textures.Length)]);
        }
    }

    void Update()
    {

    }
}
