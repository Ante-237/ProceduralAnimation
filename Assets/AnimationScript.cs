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
    public Transform TopRightLeg;
    public Transform RightLeg;
    public Transform RightLegMid;
    [Space(10)]
    public Transform TopLeftLeg;
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


    [Header("TestingLeft")]
    public Transform FinalPoint;
    public Transform destinationTopLeg;
    public Transform originalTopLeg;

    [Header("TestingRight")]
    public Transform FinalPointRight;
    public Transform destinationTopLegRight;
    public Transform originalTopLegRight;

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

    private float Timewait = 0.5f;
    private float SecondSwitch = 1.0f;
    private float secondInitial = 0.0f;
    private float initialTimeLeg = 0.0f;
    int state = 0;

    private void Update()
    {
        if (startTiming)
        {
            blendFade += Time.deltaTime * 15;
            sMeshRendererFace.SetBlendShapeWeight(currentIndex, blendFade);
        }

        if(secondInitial > SecondSwitch)
        {
            state = 1 - state;
            secondInitial = 0;
            initialTimeLeg = 0;
        }

        secondInitial += Time.deltaTime;

        switch (state)
        {
            case 0:

                LegMoveSequenceUp(LeftLeg, LeftLegAngleRotateMin, LeftLegAngleRotateMax, Time.deltaTime * bodySpeed);
                LegMoveSequence(LeftLegMid, LeftLegMidAngleRotateMin, LeftLegMidAngleRotateMax, Time.deltaTime * bodySpeed);

                RightMoveSequenceUp(RightLegMid, LeftLegAngleRotateMax, LeftLegAngleRotateMin, Time.deltaTime * bodySpeed);
                RightMoveSequence(RightLeg, LeftLegAngleRotateMax, LeftLegAngleRotateMin, Time.deltaTime * bodySpeed);


                if (initialTimeLeg > Timewait)
                {
                    TopLegMoveSequence(TopLeftLeg, LeftLegMidAngleRotateMin, LeftLegMidAngleRotateMin, Time.deltaTime * bodySpeed);
                }

                initialTimeLeg += Time.deltaTime;
                break;
            case 1:
                LegMoveSequence(LeftLegMid, LeftLegMidAngleRotateMax, LeftLegMidAngleRotateMin, Time.deltaTime * bodySpeed);
                TopLegMoveSequenceReverse(TopLeftLeg, LeftLegAngleRotateMax, LeftLegAngleRotateMin, Time.deltaTime * bodySpeed);
                
                initialTimeLeg += Time.deltaTime;
                break;
        }

        

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

    public void RightMoveSequenceUp(Transform Leg, float AngleMin, float AngleMax, float deltaTime)
    {
        // Leg.transform.rotation = Vector3.Lerp(new Quaternion.Euler(AngleMax, 0, 0), new Vector3(AngleMax, 0, 0), deltaTime);
        // Leg.transform.localRotation =  Quaternion.Lerp(Quaternion.Euler(AngleMin, Leg.transform.localRotation.y, Leg.transform.localRotation.z),Quaternion.Euler(AngleMax, Leg.transform.rotation.y, Leg.transform.rotation.z), deltaTime);
        // Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, FinalPoint.localRotation, deltaTime);
        Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, FinalPointRight.localRotation, deltaTime);
    }

    public void LegMoveSequenceUp(Transform Leg, float AngleMin, float AngleMax, float deltaTime)
    {
        // Leg.transform.rotation = Vector3.Lerp(new Quaternion.Euler(AngleMax, 0, 0), new Vector3(AngleMax, 0, 0), deltaTime);
        // Leg.transform.localRotation =  Quaternion.Lerp(Quaternion.Euler(AngleMin, Leg.transform.localRotation.y, Leg.transform.localRotation.z),Quaternion.Euler(AngleMax, Leg.transform.rotation.y, Leg.transform.rotation.z), deltaTime);
        // Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, FinalPoint.localRotation, deltaTime);
        Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, FinalPoint.localRotation, deltaTime);
    }

    public void RightMoveSequence(Transform Leg, float AngleMin, float AngleMax, float deltaTime)
    {
        Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, destinationTopLegRight.localRotation, deltaTime);
    }


    public void LegMoveSequence(Transform Leg, float AngleMin,float AngleMax, float deltaTime)
    {
        // Leg.transform.rotation = Vector3.Lerp(new Quaternion.Euler(AngleMax, 0, 0), new Vector3(AngleMax, 0, 0), deltaTime);
        // Leg.transform.localRotation =  Quaternion.Lerp(Quaternion.Euler(AngleMin, Leg.transform.localRotation.y, Leg.transform.localRotation.z),Quaternion.Euler(AngleMax, Leg.transform.rotation.y, Leg.transform.rotation.z), deltaTime);
        // Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, FinalPoint.localRotation, deltaTime);
        Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, Quaternion.Euler(AngleMax, Leg.localRotation.y, Leg.localRotation.z), deltaTime);
    }


    public void TopLegMoveSequence(Transform Leg, float AngleMin, float AngleMax, float deltaTime)
    {
        // Leg.transform.rotation = Vector3.Lerp(new Quaternion.Euler(AngleMax, 0, 0), new Vector3(AngleMax, 0, 0), deltaTime);
        // Leg.transform.localRotation =  Quaternion.Lerp(Quaternion.Euler(AngleMin, Leg.transform.localRotation.y, Leg.transform.localRotation.z),Quaternion.Euler(AngleMax, Leg.transform.rotation.y, Leg.transform.rotation.z), deltaTime);
        // Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, FinalPoint.localRotation, deltaTime);
        Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, destinationTopLeg.localRotation, deltaTime);
    }


    // reverse top movements

    public void TopLegMoveSequenceReverse(Transform Leg, float AngleMin, float AngleMax, float deltaTime)
    {
        // Leg.transform.rotation = Vector3.Lerp(new Quaternion.Euler(AngleMax, 0, 0), new Vector3(AngleMax, 0, 0), deltaTime);
        // Leg.transform.localRotation =  Quaternion.Lerp(Quaternion.Euler(AngleMin, Leg.transform.localRotation.y, Leg.transform.localRotation.z),Quaternion.Euler(AngleMax, Leg.transform.rotation.y, Leg.transform.rotation.z), deltaTime);
        // Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, FinalPoint.localRotation, deltaTime);
        Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, originalTopLeg.localRotation, deltaTime);
    }
}
