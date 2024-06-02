using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands.Samples.GestureSample;

public class CatapultModel
{


    public CatapultModel(ProjectileProperties atrs, Transform spawnPoint, StaticHandGesture grabGesture, Projectile ProjectilePrefab)
	{
        this.projectileProperties = atrs;
		this.spawnPoint = spawnPoint;
		this.grabGesture = grabGesture;

        pooler = new ProjectilePooler(ProjectilePrefab);
	}

    #region Properties
    public ProjectileProperties projectileProperties;

    private CatapultController controller = null;
    public Transform spawnPoint { get; private set; }

    public StaticHandGesture grabGesture { get; private set; }

    public ProjectilePooler pooler { get; private set; }
    #region CatapultData

   
    public Transform LeftKnot { get; private set; }
    public Transform RightKnot { get; private set; }
    
    // This is the transform of Projectile's dummy situated in catapult itself. Not a real projectile.
    public Transform Projectile { get; private set; }
    #endregion

    #endregion

    public void InitializeCatapultData(Transform leftKnot , Transform rightKnot, Transform Projectile)
    {
        this.LeftKnot = leftKnot;
        this.RightKnot = rightKnot;
        this.Projectile = Projectile;
    }

    public void SetController(CatapultController controller)
    {
        this.controller = controller;
    }

}
