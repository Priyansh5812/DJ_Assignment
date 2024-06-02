using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Show : Swing
{
    private float moveSpeed;
    private Transform placePointHolder;
    public Show(float moveSpeed , Transform placePointHolder) : base()
    {   
        this.moveSpeed = moveSpeed;
        this.placePointHolder = placePointHolder;
        foreach (Transform i in placePointHolder)
        {
            PlacePoint temp = null;
            if (i.gameObject.TryGetComponent<PlacePoint>(out temp))
                placePoints.Add(temp);    
        }
    }

    public override void DisableSwing()
    {

    }

    public override void EnableSwing()
    {

    }

    public override void UpdateSwing()
    {
        foreach (var i in placePoints)
        {
            
            i.transform.Translate(i.transform.right * moveSpeed * Time.deltaTime);
            
        }
    }
}
