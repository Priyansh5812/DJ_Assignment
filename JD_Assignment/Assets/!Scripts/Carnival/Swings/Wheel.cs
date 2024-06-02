using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wheel : Swing
{
    private Animator anim;
    public Wheel(Animator anim) : base()
    {
        this.anim = anim;
    }


    public override void DisableSwing()
    {   
        anim.StopPlayback();
    }

    public override void EnableSwing()
    {
        anim.CrossFadeInFixedTime("RotateWheel", 0.5f);
    }

    public override void UpdateSwing()
    {
        
    }
}
