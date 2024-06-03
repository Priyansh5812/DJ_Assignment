using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class XR_Recenter : MonoBehaviour
{

    [SerializeField] private Transform head, origin, target;

    void Start()
    {
        RecenterRig();
    }

    private void RecenterRig()
    { 
        XROrigin rig = origin.GetComponent<XROrigin>();
        rig.MoveCameraToWorldLocation(target.position);
        rig.MatchOriginUpCameraForward(target.up, target.forward);
    }
}
