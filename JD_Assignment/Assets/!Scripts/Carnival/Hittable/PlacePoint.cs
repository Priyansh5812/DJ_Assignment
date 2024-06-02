using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacePoint : MonoBehaviour
{
    public SwingDriver swingDriver;
    private HittableFactory fact;
    
    void Start()
    {
        fact = swingDriver.GetFactory();
    }


}
