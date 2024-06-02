using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Merry_Go_Round : Swing
{
    private float rotationSpeed;
    private bool isClockwise;
    private Transform rotationObject;

    
    public Merry_Go_Round(float rotationSpeed, bool isClockwise, Transform rotationObject,  HittableFactory fact) : base(fact)
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
        rotationObject?.Rotate(Vector3.forward * rotationSpeed * ((isClockwise) ? (1) : (-1)) * Time.deltaTime);
       
    }
}
