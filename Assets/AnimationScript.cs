using System;
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
    [Range(0, 100f)] public float bodySpeed = 0.2f;
    [Range(0, 20f)] public float movementSpeed = 0.5f;

    [Range(0, 100f)] public float TurnSpeed = 10f;

    [Range(0, 10f)] public float CoolDownTimeToTurn = 1.5f;



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
    public Transform originalRightLower;


    [Header("Body Shake Movements")]
    public Transform BodyTransform;
    public Transform PingForward;
    public Transform PingBackward;
    [SerializeField, Range(0.0f, 4.0f)] private float BodyChangeTime = 2.0f;

    int currentIndex = 0;
    float initialCoolDown = 0.0f;



    private void Start()
    {
        //SetFaceHappy();
        StartCoroutine(MovethroughExpressions());
    }

    //private void SetFaceHappy()
    //{
    //    sMeshRendererFace.SetBlendShapeWeight(4, 100);
    //}

    private float Timewait = 0.25f;
    private float SecondSwitch = 0.5f;
    private float secondInitial = 0.0f;
    private float initialTimeLeg = 0.0f;
    int state = 0;



    private float bodyMoveTiming = 0.0f;
    private bool canTurn = false;
    private bool originalTurn = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("obstacle"))
        {
            Debug.LogWarning("Collision About to happen");
            canTurn = true;
            originalTurn = false;
           
        }
    }



    private void Update()
    {

        transform.localPosition +=   -transform.right * Time.deltaTime * movementSpeed;


        if (originalTurn)
        {
            initialCoolDown = 0.0f;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * TurnSpeed);
        }

        if (canTurn)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 90, 0), Time.deltaTime * TurnSpeed);

            if(initialCoolDown > CoolDownTimeToTurn)
            {
                canTurn = false;
                originalTurn = true;
            }

            initialCoolDown += Time.deltaTime;
        }





      



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

                RightMoveSequenceUpReverse(RightLegMid, LeftLegAngleRotateMax, LeftLegAngleRotateMin, Time.deltaTime * bodySpeed);
                RightMoveSequenceReverse(RightLeg, LeftLegAngleRotateMax, LeftLegAngleRotateMin, Time.deltaTime * bodySpeed);

                PingPongBodyMovementsF(Time.deltaTime * bodySpeed);

                if (initialTimeLeg > Timewait)
                {
                  //  TopLegMoveSequence(TopLeftLeg, LeftLegMidAngleRotateMin, LeftLegMidAngleRotateMin, Time.deltaTime * bodySpeed);
                }

                initialTimeLeg += Time.deltaTime;
                break;
            case 1:
                LegMoveSequence(LeftLegMid, LeftLegMidAngleRotateMax, LeftLegMidAngleRotateMin, Time.deltaTime * bodySpeed);
                TopLegMoveSequenceReverse(TopLeftLeg, LeftLegAngleRotateMax, LeftLegAngleRotateMin, Time.deltaTime * bodySpeed);


             
                RightMoveSequenceUp(RightLegMid, LeftLegAngleRotateMax, LeftLegAngleRotateMin, Time.deltaTime * bodySpeed);
                RightMoveSequence(RightLeg, LeftLegAngleRotateMax, LeftLegAngleRotateMin, Time.deltaTime * bodySpeed);

                PingPongBodyMovementsB(Time.deltaTime * bodySpeed);

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


    public void RightMoveSequence(Transform Leg, float AngleMin, float AngleMax, float deltaTime)
    {
        Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, destinationTopLegRight.localRotation, deltaTime);
    }

    // reversing the right legs

    public void RightMoveSequenceUpReverse(Transform Leg, float AngleMin, float AngleMax, float deltaTime)
    {
        // Leg.transform.rotation = Vector3.Lerp(new Quaternion.Euler(AngleMax, 0, 0), new Vector3(AngleMax, 0, 0), deltaTime);
        // Leg.transform.localRotation =  Quaternion.Lerp(Quaternion.Euler(AngleMin, Leg.transform.localRotation.y, Leg.transform.localRotation.z),Quaternion.Euler(AngleMax, Leg.transform.rotation.y, Leg.transform.rotation.z), deltaTime);
        // Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, FinalPoint.localRotation, deltaTime);
        Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, originalRightLower.localRotation, deltaTime);
    }


    public void RightMoveSequenceReverse(Transform Leg, float AngleMin, float AngleMax, float deltaTime)
    {
        Leg.transform.localRotation = Quaternion.Lerp(Leg.transform.localRotation, originalTopLeg.localRotation, deltaTime);

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


    private void PingPongBodyMovementsF(float deltaTime)
    {
        BodyTransform.localRotation = Quaternion.Lerp(BodyTransform.localRotation, PingForward.localRotation, deltaTime);
    }


    private void PingPongBodyMovementsB(float deltaTime)
    {
        BodyTransform.localRotation = Quaternion.Lerp(BodyTransform.localRotation, PingBackward.localRotation, deltaTime);
    }
}
