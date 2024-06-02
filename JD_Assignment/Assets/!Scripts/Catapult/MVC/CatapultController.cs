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
	public bool isCatapultValid = false;
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
			isCatapultValid = true;
		});

		model.grabGesture.gestureEnded.AddListener(() =>
		{
			view.OnToggleCatapult(false);
			isCatapultValid = false;
		});

		//-------------Projectile Interactable Init--------------
		ProjectileEvent.Service.onReleaseProjectile.AddListener(ToggleProjectileGrabState);
		ProjectileEvent.Service.onReleaseProjectile.AddListener(LaunchProjectile);

		ProjectileEvent.Service.onGrabProjectile.AddListener(ToggleProjectileGrabState);

		//------------ Controller Runtime Init------------------
		view.ControllerRuntime.AddListener(ResetProjectile);
		view.ControllerRuntime.AddListener(PredictTrajectory);
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


    public void PredictTrajectory()
    {

        Vector3 velocity = GetInitialDirection() * ((model.projectileProperties.initialSpeed * GetSpeedModifier()) / model.projectileProperties.mass); // direction * speed per mass
        Vector3 position = model.Projectile.position;
        Vector3 nextPosition;
        float overlap;
        view.UpdateLineRenderer(model.projectileProperties.maxPoints, (0, position)); // Setting initial pos  for line renderer
        for (int i = 1; i < model.projectileProperties.maxPoints; i++)
        {
            velocity = CalculateVelocity(velocity, model.projectileProperties.drag); // Adding effect of gravity and Air drag
			nextPosition = position + velocity * Time.fixedDeltaTime;
            overlap = Vector3.Distance(position, nextPosition) * model.projectileProperties.rayOverlap;
            if (Physics.Raycast(position, velocity.normalized, out RaycastHit hit, overlap))
            {
                view.UpdateLineRenderer(i, (i - 1, hit.point));
                break;
            }
            position = nextPosition;
            view.UpdateLineRenderer(model.projectileProperties.maxPoints, (i, position));
        }

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
		velocity += Physics.gravity * Time.fixedDeltaTime;
        velocity *= Mathf.Clamp01(1f - drag * 0.01333f);
        return velocity;
    }

	public float GetSpeedModifier()
	{
		float res;
        Vector3 midPoint = (model.LeftKnot.position + model.RightKnot.position) / 2;
        Vector3 projToMid = midPoint - model.Projectile.transform.position;
		res = Mathf.InverseLerp(0f, 0.35f, projToMid.magnitude);
        return res;
    }

	private void LaunchProjectile(bool val)
	{	
		IPoolObject<Projectile> pooler = model.pooler as IPoolObject<Projectile>;
		Rigidbody rb = pooler.CreateOrDequeueObject().GetComponent<Rigidbody>();
		rb.transform.SetPositionAndRotation(model.Projectile.transform.position, model.LeftKnot.transform.rotation);
        rb.gameObject.SetActive(true);
        rb.AddForce(GetInitialDirection() * model.projectileProperties.initialSpeed * GetSpeedModifier(), ForceMode.Impulse);

		view.StartCoroutine(view.NextProjectileLaunchTimeout());

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
        ProjectileEvent.Service.onReleaseProjectile.RemoveListener(LaunchProjectile);

        ProjectileEvent.Service.onGrabProjectile.RemoveListener(ToggleProjectileGrabState);

        view.ControllerRuntime.RemoveListener(ResetProjectile);
        view.ControllerRuntime.RemoveListener(PredictTrajectory);

        model.grabGesture.gestureEnded.RemoveListener(RetainProjectileParent);
    }


	

}
