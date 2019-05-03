using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Configuration : MonoBehaviour {
    enum ConfigurationState {HandSelection, Horizontal, Vertical, Depth};
    ConfigurationState configurationState = ConfigurationState.HandSelection;
    public GameObject[] horizontalLimits;
    public GameObject horizontalIndicator;
    public GameObject horizontalPanel;
    public GameObject horizontalGrid;

    public GameObject[] verticalLimits;
    public GameObject verticalIndicator;
    public GameObject verticalPanel;
    public GameObject verticalGrid;

    public GameObject[] depthLimits;
    public GameObject depthIndicator;
    public GameObject depthPanel;
    public GameObject depthGrid;
    // Use this for initialization
    void Start () {
        ShowHorIndicators(false);
        ShowVerIndicators(false);
        ShowDepthIndicators(false);
    }

    public void ShowHorIndicators(bool show)
    {
        foreach(GameObject g in horizontalLimits)
        {
            g.SetActive(show);
        }
        horizontalIndicator.SetActive(show);
        horizontalPanel.SetActive(show);
        horizontalGrid.SetActive(show);
    }
    public void ShowVerIndicators(bool show)
    {
        foreach (GameObject g in verticalLimits)
        {
            g.SetActive(show);
        }
        verticalIndicator.SetActive(show);
        verticalPanel.SetActive(show);
        verticalGrid.SetActive(show);
    }
    public void ShowDepthIndicators(bool show)
    {
        foreach (GameObject g in depthLimits)
        {
            g.SetActive(show);
        }
        depthIndicator.SetActive(show);
        depthPanel.SetActive(show);
        depthGrid.SetActive(show);
    }

    public void resetPositionHor()
    {
        foreach (GameObject g in horizontalLimits)
        {
            g.transform.position = horizontalIndicator.transform.position;
        }
    }
    public void resetPositionVer()
    {
        foreach (GameObject g in verticalLimits)
        {
            g.transform.position = verticalIndicator.transform.position;
        }
    }
    public void resetPositionDepth()
    {
        foreach (GameObject g in depthLimits)
        {
            g.transform.position = depthIndicator.transform.position;
        }
    }

    public void HandSelectionLeft()
    {
        PlayerPrefs.SetInt("handSelected", -1);
        configurationState = ConfigurationState.Horizontal;
        ShowHorIndicators(true);
        resetPositionHor();
        AstraManager.instance.objectToMove = horizontalIndicator;
        AstraManager.instance.lockMovX = false;
        AstraManager.instance.lockMovY = true;
        AstraManager.instance.lockMovZ = true;
        AstraManager.instance.SetHand();
    }

    public void HandSelectionRight()
    {
        PlayerPrefs.SetInt("handSelected", 1);
        configurationState = ConfigurationState.Horizontal;
        ShowHorIndicators(true);
        resetPositionHor();
        AstraManager.instance.objectToMove = horizontalIndicator;
        AstraManager.instance.lockMovX = false;
        AstraManager.instance.lockMovY = true;
        AstraManager.instance.lockMovZ = true;
        AstraManager.instance.SetHand();
    }

    public void backToHandSelection()
    {
        configurationState = ConfigurationState.HandSelection;
        ShowHorIndicators(false);
    }
    public void backToHorSelection()
    {
        configurationState = ConfigurationState.Horizontal;
        ShowHorIndicators(true);
        ShowVerIndicators(false);
        ShowDepthIndicators(false);
        AstraManager.instance.objectToMove = horizontalIndicator;
        AstraManager.instance.lockMovX = false;
        AstraManager.instance.lockMovY = true;
        AstraManager.instance.lockMovZ = true;
        AstraManager.instance.resetObjectToMove();
        resetPositionHor();
    }
    public void backToVerSelection()
    {
        configurationState = ConfigurationState.Vertical;
        ShowHorIndicators(false);
        ShowVerIndicators(true);
        ShowDepthIndicators(false);
        AstraManager.instance.objectToMove = verticalIndicator;
        AstraManager.instance.lockMovX = true;
        AstraManager.instance.lockMovY = false;
        AstraManager.instance.lockMovZ = true;
        AstraManager.instance.resetObjectToMove();
        resetPositionVer();
    }

    public void OKHorSelection()
    {
        configurationState = ConfigurationState.Vertical;
        ShowHorIndicators(false);
        ShowVerIndicators(true);
        ShowDepthIndicators(false);
        AstraManager.instance.objectToMove = verticalIndicator;
        AstraManager.instance.lockMovX = true;
        AstraManager.instance.lockMovY = false;
        AstraManager.instance.lockMovZ = true;
        AstraManager.instance.resetObjectToMove();
        resetPositionVer();
    }
    public void OKVerSelection()
    {
        configurationState = ConfigurationState.Depth;
        ShowVerIndicators(false);
        ShowDepthIndicators(true);
        AstraManager.instance.objectToMove = depthIndicator;
        AstraManager.instance.lockMovX = true;
        AstraManager.instance.lockMovY = true;
        AstraManager.instance.lockMovZ = false;
        AstraManager.instance.resetObjectToMove();
        resetPositionDepth();
    }
    public void OKDepthSelection()
    {
        PlayerPrefs.SetFloat("limitHorMin", horizontalLimits[0].transform.position.x);
        PlayerPrefs.SetFloat("limitHorMax", horizontalLimits[1].transform.position.x);
        PlayerPrefs.SetFloat("limitVerMin", verticalLimits[0].transform.position.y);
        PlayerPrefs.SetFloat("limitVerMax", verticalLimits[1].transform.position.y);
        PlayerPrefs.SetFloat("limitDepthMin", depthLimits[0].transform.position.z);
        PlayerPrefs.SetFloat("limitDepthMax", depthLimits[1].transform.position.z);

        DatabaseManager.instance.SaveConfiguration();
        SceneManager.LoadScene("GameGrid");
    }

    // Update is called once per frame
    void Update () {
        switch (configurationState)
        {
            case ConfigurationState.Horizontal:
                if (horizontalLimits[0].transform.position.x > horizontalIndicator.transform.position.x)
                    horizontalLimits[0].transform.position = new Vector3(horizontalIndicator.transform.position.x, horizontalLimits[0].transform.position.y, horizontalLimits[0].transform.position.z);
                if (horizontalLimits[1].transform.position.x < horizontalIndicator.transform.position.x)
                    horizontalLimits[1].transform.position = new Vector3(horizontalIndicator.transform.position.x, horizontalLimits[1].transform.position.y, horizontalLimits[1].transform.position.z);
                break;
            case ConfigurationState.Vertical:
                if (verticalLimits[0].transform.position.y > verticalIndicator.transform.position.y)
                    verticalLimits[0].transform.position = new Vector3(verticalLimits[0].transform.position.x, verticalIndicator.transform.position.y, verticalLimits[0].transform.position.z);
                if (verticalLimits[1].transform.position.y < verticalIndicator.transform.position.y)
                    verticalLimits[1].transform.position = new Vector3(verticalLimits[1].transform.position.x, verticalIndicator.transform.position.y, verticalLimits[1].transform.position.z);
                break;
            case ConfigurationState.Depth:
                if (depthLimits[0].transform.position.z > depthIndicator.transform.position.z)
                    depthLimits[0].transform.position = new Vector3(depthLimits[0].transform.position.x, depthLimits[0].transform.position.y, depthIndicator.transform.position.z);
                if (depthLimits[1].transform.position.z < depthIndicator.transform.position.z)
                    depthLimits[1].transform.position = new Vector3(depthLimits[1].transform.position.x, depthLimits[1].transform.position.y, depthIndicator.transform.position.z);
                break;
        }
	}
}
