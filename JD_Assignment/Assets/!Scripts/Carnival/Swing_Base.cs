using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public abstract class Swing
{

    protected List<PlacePoint> placePoints; // TODO: PlacePoint.cs

    protected Swing()
    {
        placePoints = new List<PlacePoint>();
    }

    public abstract void EnableSwing();


    public abstract void UpdateSwing();


    public abstract void DisableSwing();

    


}
