using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ProjectileGrabInteractable : XRGrabInteractable
{


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        ProjectileEvent.Service.onGrabProjectile.InvokeEvent(true);

    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        ProjectileEvent.Service.onReleaseProjectile.InvokeEvent(false);
        
    }




}
