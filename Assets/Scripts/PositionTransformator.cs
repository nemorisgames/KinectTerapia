using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTransformator : MonoBehaviour
{
    public static PositionTransformator Instance;
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
    public Vector3 positionReal;
    private Vector3 positionInit;
    public bool forcePosition = true;
    public bool localPosition = false;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    void Start()
    {
        PlayerPrefs.SetFloat("limitHorMax", 10);
        PlayerPrefs.SetFloat("limitHorMin", -10);
        PlayerPrefs.SetFloat("limitVerMax", 10);
        PlayerPrefs.SetFloat("limitVerMin", -10);
        PlayerPrefs.SetFloat("limitDepthMax", 10);
        PlayerPrefs.SetFloat("limitDepthMin", -10);

        horDifference = horLimits[1] - horLimits[0];
        verDifference = verLimits[1] - verLimits[0];
        depthDifference = depthLimits[1] - depthLimits[0];

        horDifferenceConfig = PlayerPrefs.GetFloat("limitHorMax") - PlayerPrefs.GetFloat("limitHorMin");
        verDifferenceConfig = PlayerPrefs.GetFloat("limitVerMax") - PlayerPrefs.GetFloat("limitVerMin");
        depthDifferenceConfig = PlayerPrefs.GetFloat("limitDepthMax") - PlayerPrefs.GetFloat("limitDepthMin");

        if (localPosition)
            positionInit = transform.localPosition;
        else
            positionInit = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //porcentaje de posicion con respecto a lo almacenado
        float percentageLimitHor = ((referenceObject.position.x - PlayerPrefs.GetFloat("limitHorMin")) / horDifferenceConfig);
        float positionHorReal = (percentageLimitHor * horDifference + horLimits[0]);
        float percentageLimitVer = ((referenceObject.position.y - PlayerPrefs.GetFloat("limitVerMin")) / verDifferenceConfig);
        float positionVerReal = (percentageLimitVer * verDifference + verLimits[0]);
        float percentageLimitDepth = ((referenceObject.position.z - PlayerPrefs.GetFloat("limitDepthMin")) / depthDifferenceConfig);
        float positionDepthReal = (percentageLimitDepth * depthDifference + depthLimits[0]);
        positionReal = new Vector3(positionHorReal, positionVerReal, positionDepthReal);
        if (!forcePosition)
            return;
        if (localPosition)
            transform.localPosition = positionInit + (positionInit == positionReal ? Vector3.zero : positionReal);
        else
            transform.position = positionInit + (positionInit == positionReal ? Vector3.zero : positionReal);

    }

    public void ForcePosition(bool b)
    {
        forcePosition = b;
    }
}
