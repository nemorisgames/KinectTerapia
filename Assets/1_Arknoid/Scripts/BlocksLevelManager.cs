using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksLevelManager : MonoBehaviour {
    public GameObject level1Blocks;
    public GameObject level2Blocks;
    public GameObject level3Blocks;
    
    // Use this for initialization
    void Start () {
        level1Blocks.SetActive(true);
        level2Blocks.SetActive(false);
        level3Blocks.SetActive(false);
    }

    public void ShowBlocks()
    {
        if(GameManager.instance.currentLevel == 2)
        {
            ShowLevel2Blocks();
        }
        if (GameManager.instance.currentLevel == 3)
        {
            ShowLevel3Blocks();
        }
    }

    public void ShowLevel2Blocks()
    {
        level2Blocks.SetActive(true);
    }

    public void ShowLevel3Blocks()
    {
        level3Blocks.SetActive(true);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
