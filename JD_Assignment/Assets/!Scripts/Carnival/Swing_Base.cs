using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public abstract class Swing
{
    protected HittableFactory factory;
    protected List<PlacePoint> placePoints; // TODO: PlacePoint.cs

    protected Swing(HittableFactory factory)
    {
        this.factory = factory;
        placePoints = new List<PlacePoint>();
    }

    public abstract void EnableSwing();


    public abstract void UpdateSwing();


    public abstract void DisableSwing();

    


}
