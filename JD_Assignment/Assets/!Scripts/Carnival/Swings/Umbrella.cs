using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella : Swing
{
    private float rotationSpeed;
    private bool isClockwise;
    private Transform rotationObject;


    public Umbrella(float rotationSpeed, bool isClockwise, Transform rotationObject) : base()
    {
        this.rotationSpeed = rotationSpeed;
        this.isClockwise = isClockwise;
        this.rotationObject = rotationObject;
    }


    public override void DisableSwing()
    {
        
    }

    public override void EnableSwing()
    {
        
    }

    public override void UpdateSwing()
    {
        rotationObject?.Rotate(rotationObject.transform.up * rotationSpeed * ((isClockwise) ? (1) : (-1)) * Time.deltaTime);
    }

}
