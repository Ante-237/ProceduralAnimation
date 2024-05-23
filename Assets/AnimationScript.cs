using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    [Header("Facial Expressions")]
    public SkinnedMeshRenderer sMeshRendererFace;
    public bool startTiming;
    public float blendFade = 0;

    [Header("Speed")]
    [Range(0, 10f)] public float bodySpeed = 0.2f;


    [Header("Body Parts")]
    public Transform RightLeg;
    public Transform RightLegMid;
    public Transform LeftLeg;
    public Transform LeftLegMid;



    [Header("Clamp Angle Values")]
    [Range(-360, 360)] public float RightLegAngleRotateMin;
    [Range(-360, 360)] public float LeftLegAngleRotateMin;
    [Range(-360, 360)] public float RightLegMidAngleRotateMin;
    [Range(-360, 360)] public float LeftLegMidAngleRotateMin;
    [Range(-360, 360)] public float RightLegAngleRotateMax;
    [Range(-360, 360)] public float LeftLegAngleRotateMax;
    [Range(-360, 360)] public float RightLegMidAngleRotateMax;
    [Range(-360, 360)] public float LeftLegMidAngleRotateMax;



    [Header("Update Transform State")]
    public bool UpdateRightMid;
    public bool UpdateRight;
    public bool UpdateLeft;
    public bool UpdateLeftMid;


    [Header("Testing")]
    public Transform FinalPoint;

    int currentIndex = 0;




    private void Start()
    {
        //SetFaceHappy();
        StartCoroutine(MovethroughExpressions());
    }

    private void SetFaceHappy()
    {
        sMeshRendererFace.SetBlendShapeWeight(4, 100);
    }

    private void Update()
    {
        if (startTiming)
        {
            blendFade += Time.deltaTime * 15;
            sMeshRendererFace.SetBlendShapeWeight(currentIndex, blendFade);
        }

        LegMoveSequenceUp(LeftLeg, LeftLegAngleRotateMin, LeftLegAngleRotateMax, Time.deltaTime * bodySpeed);

       // LegMoveSequence(LeftLeg, LeftLegAngleRotateMin, LeftLegAngleRotateMax, Time.deltaTime * bodySpeed);
        LegMoveSequence(LeftLegMid, LeftLegMidAngleRotateMin, LeftLegMidAngleRotateMax, Time.deltaTime * bodySpeed);
       

        LegUpdateTimer();
    }

    IEnumerator MovethroughExpressions()
    {
        while (true)
        {
            
            startTiming = true;
            yield return new WaitForSeconds(6);
            startTiming = false;
            blendFade = 0;
            sMeshRendererFace.SetBlendShapeWeight(currentIndex, 0);
            if(currentIndex == 7)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
        }
    }

    private void LegUpdateTimer()
    {

    }

    public void LegMoveSequenceUp(Transform Leg, float AngleMin, float AngleMax, float deltaTime)
    {
        // Leg.transform.rotation = Vector3.Lerp(new Quaternion.Euler(AngleMax, 0, 0), new Vector3(AngleMax, 0, 0), deltaTime);
        // Leg.transform.localRotation =  Quaternion.Lerp(Quaternion.Euler(AngleMin, Leg.transform.localRotation.y, Leg.transform.localRotation.z),Quaternion.Euler(AngleMax, Leg.transform.rotation.y, Leg.transform.rotation.z), deltaTime);
        // Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, FinalPoint.localRotation, deltaTime);
        Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, FinalPoint.localRotation, deltaTime);
    }


    public void LegMoveSequence(Transform Leg, float AngleMin,float AngleMax, float deltaTime)
    {
        // Leg.transform.rotation = Vector3.Lerp(new Quaternion.Euler(AngleMax, 0, 0), new Vector3(AngleMax, 0, 0), deltaTime);
        // Leg.transform.localRotation =  Quaternion.Lerp(Quaternion.Euler(AngleMin, Leg.transform.localRotation.y, Leg.transform.localRotation.z),Quaternion.Euler(AngleMax, Leg.transform.rotation.y, Leg.transform.rotation.z), deltaTime);
        // Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, FinalPoint.localRotation, deltaTime);
        Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, Quaternion.Euler(AngleMax, Leg.localRotation.y, Leg.localRotation.z), deltaTime);
    }
    
}
