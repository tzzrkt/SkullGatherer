using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boneanimator : MonoBehaviour
{
    public Transform leftleg;
    public Transform rightleg;
    public controller cont;

    public bool ismoving;

    public float MaxAngleDeflection = 30.0f;
    public float SpeedOfPendulum = 5.0f;

    public float offsetAngle = -90f;

    public float globalOffset = 1.5f;
    // Start is called before the first frame update
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {
        ismoving = cont.isMoving;
        if (ismoving)
        {
            float anglel = MaxAngleDeflection * Mathf.Sin(Time.time * SpeedOfPendulum + globalOffset) + offsetAngle;
            float angler = MaxAngleDeflection * Mathf.Sin(Time.time * SpeedOfPendulum - globalOffset) + offsetAngle;

            leftleg.localRotation = Quaternion.Lerp(leftleg.localRotation, Quaternion.Euler(anglel,0,0), .5f);
            rightleg.localRotation = Quaternion.Lerp(rightleg.localRotation, Quaternion.Euler(angler, 0, 0), .5f);
        }
        if (!ismoving)
        {
            leftleg.localRotation = Quaternion.Lerp(leftleg.localRotation, Quaternion.Euler(-90, 0, 0),.5f);
            rightleg.localRotation = Quaternion.Lerp(rightleg.localRotation, Quaternion.Euler(-90, 0, 0), .5f);
        }
    }
}
