using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class RandomTexture : MonoBehaviour
{
    MeshRenderer meshRenderer;
    //Textures that will be used randomly in this spriteRenderer
    public Texture[] textures;
    public bool applyOnDetailMap = true;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Generate();
    }

    //Generate and assign one of the textures randomly
    public void Generate()
    {
        if (textures.Length > 0)
        {
            Texture texture = textures[Random.Range(0, textures.Length)];
            meshRenderer.material.SetTexture("_MainTex", texture);
            //Assign one texture for the main texture and another to the detail texture to increase the randomness in the resulting material
            if (applyOnDetailMap)
                meshRenderer.material.SetTexture("_DetailAlbedoMap", textures[Random.Range(0, textures.Length)]);
        }
    }

    void Update()
    {

    }
}
