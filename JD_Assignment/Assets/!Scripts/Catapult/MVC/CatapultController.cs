using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultController
{

	//References
	CatapultView view = null;
	CatapultModel model = null;

	//Other Fields
	private bool isProjectileGrabbed = false;
	
    public CatapultView View
	{
		get 
		{
			return view;
		}
	}

	public CatapultModel Model
	{
		get 
		{
			return model;	
		}
	}

	public CatapultController(CatapultModel model , CatapultView view)
	{
		this.model = model;
		this.model.SetController(this);

		Transform spawnPoint = model.spawnPoint;

		this.view = GameObject.Instantiate(view, spawnPoint.position, spawnPoint.rotation, spawnPoint.parent);
		this.view.SetController(this);
		
		// For Late Initializations
		LazyInit();
	}

	private void LazyInit()
	{

		//Suppying Model with Necessary Catapult Data
		model.InitializeCatapultData(this.view.System.Find("LeftKnot"), this.view.System.Find("RightKnot"), this.view.System.Find("Projectile"));

		//-------------Grab Gesture Listener Init--------------
		model.grabGesture.gesturePerformed.AddListener(() => 
		{
			view.OnToggleCatapult(true);
		});

		model.grabGesture.gestureEnded.AddListener(() =>
		{
			view.OnToggleCatapult(false);
		});

		//-------------Projectile Interactable Init--------------
		ProjectileEvent.Service.onReleaseProjectile.AddListener(ToggleProjectileGrabState);
		ProjectileEvent.Service.onGrabProjectile.AddListener(ToggleProjectileGrabState);

		//------------ Controller Runtime Init------------------
		view.ControllerRuntime.AddListener(ResetProjectile);

		//---------------------On Gesture Init-------------------
		/// FIX : Projectile does not retain its parent after grabbing the projectile and gets lost 

		model.grabGesture.gestureEnded.AddListener(RetainProjectileParent);
		
	}


	public Vector3 GetInitialDirection()
	{	

		Vector3 midPoint = (model.LeftKnot.position + model.RightKnot.position) / 2;

		if (midPoint == model.Projectile.position)
			return model.LeftKnot.forward;

		Vector3 projToMid =  midPoint - model.Projectile.transform.position;

		return projToMid.normalized;
    }


	private void ResetProjectile()
	{
		if (isProjectileGrabbed)
			return;
		model.Projectile.position = ((model.LeftKnot.position + model.RightKnot.position) / 2);
    }

	private void ToggleProjectileGrabState(bool val)
	{
		isProjectileGrabbed = val;
	}

	private void RetainProjectileParent()
	{
		model.Projectile.parent = view.System;
	}


    public Vector3 CalculateVelocity(Vector3 velocity, float drag)
    {
        velocity += Physics.gravity * 0.01333f;
        velocity *= Mathf.Clamp01(1f - drag * 0.01333f);
        return velocity;
    }


    ~CatapultController()
	{	

		// Removing the Listeners...
        model.grabGesture.gesturePerformed.RemoveListener(() =>
        {
            view.OnToggleCatapult(true);
        });

        model.grabGesture.gestureEnded.RemoveListener(() =>
        {
            view.OnToggleCatapult(false);
        });

        ProjectileEvent.Service.onReleaseProjectile.RemoveListener(ToggleProjectileGrabState);
        ProjectileEvent.Service.onGrabProjectile.RemoveListener(ToggleProjectileGrabState);

        view.ControllerRuntime.RemoveListener(ResetProjectile);
    }


	

}
