using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTransformator : MonoBehaviour {
    public Transform referenceObject;
    public Vector2 horLimits;
    public Vector2 verLimits;
    public Vector2 depthLimits;

    public float horDifference;
    public float verDifference;
    public float depthDifference;

    public float horDifferenceConfig;
    public float verDifferenceConfig;
    public float depthDifferenceConfig;
    // Use this for initialization
    void Start () {
        horDifference = horLimits[1] - horLimits[0];
        verDifference = verLimits[1] - verLimits[0];
        depthDifference = depthLimits[1] - depthLimits[0];

        horDifferenceConfig = PlayerPrefs.GetFloat("limitHorMax") - PlayerPrefs.GetFloat("limitHorMin");
        verDifferenceConfig = PlayerPrefs.GetFloat("limitVerMax") - PlayerPrefs.GetFloat("limitVerMin");
        depthDifferenceConfig = PlayerPrefs.GetFloat("limitDepthMax") - PlayerPrefs.GetFloat("limitDepthMin");
    }
	
	// Update is called once per frame
	void Update () {

        //porcentaje de posicion con respecto a lo almacenado
        float percentageLimitHor = ((referenceObject.position.x - PlayerPrefs.GetFloat("limitHorMin")) / horDifferenceConfig);
        float positionHorReal = (percentageLimitHor * horDifference + horLimits[0]);
        float percentageLimitVer = ((referenceObject.position.y - PlayerPrefs.GetFloat("limitVerMin")) / verDifferenceConfig);
        float positionVerReal = (percentageLimitVer * verDifference + verLimits[0]);
        float percentageLimitDepth = ((referenceObject.position.z - PlayerPrefs.GetFloat("limitDepthMin")) / depthDifferenceConfig);
        float positionDepthReal = (percentageLimitDepth * depthDifference + depthLimits[0]);
        transform.position = new Vector3(positionHorReal, positionVerReal, positionDepthReal);

    }
}
