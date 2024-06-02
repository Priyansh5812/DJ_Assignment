using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultView : MonoBehaviour
{

    public Transform System { get; private set; }
    public CatapultController controller = null;
    private SkinnedMeshRenderer m_Renderer;
    private LineRenderer ribbon_Renderer;
    private ViewEventSystem controller_Runtime = null;
    public ViewEventSystem ControllerRuntime
    {
        get 
        {
            if(controller_Runtime == null)
                controller_Runtime = new ViewEventSystem();
            return controller_Runtime;
        }
        
    }

    #region Projectile

    public ProjectileProperties _trajectoryProps;
    public LineRenderer _lr;
    public float rayOverlap;
    public int maxPoints;

    #endregion




    public void SetController(CatapultController controller)
    {
        this.controller = controller;
    }


    void OnEnable()
    {
        m_Renderer = GetComponent<SkinnedMeshRenderer>();
        this.System = this.transform.GetChild(0);   
        ribbon_Renderer = this.System.GetComponent<LineRenderer>();

    }

    

    private void Start()
    {

        //---------------Line Renderer of Projectile Ray
        if (!controller.Model.Projectile.TryGetComponent<LineRenderer>(out _lr))
            Debug.LogError("Projectile Line Renderer not Initialized");

        _trajectoryProps = controller.Model.projectileProperties;
        //---------------------------
        ribbon_Renderer.positionCount = 0;
        //---------------------------
        controller.Model.Projectile.position = ((controller.Model.LeftKnot.position + controller.Model.RightKnot.position) / 2);
        //---------------------------
        ProjectileEvent.Service.onReleaseProjectile.AddListener(ToggleTrajectory);
        ProjectileEvent.Service.onGrabProjectile.AddListener(ToggleTrajectory);
        //---------------------------
        OnToggleCatapult(false);
        ToggleTrajectory(false);
       


    }

    // Update is called once per frame
    void Update()
    {   
        // Runtime Invocation for Controller Logic Executions (if any)
        controller_Runtime?.InvokeListener();
        UpdateCatapultribbon_Renderer();
    }

    private void UpdateCatapultribbon_Renderer()
    {
        ribbon_Renderer.positionCount = 3;

        CatapultModel model = controller.Model;

        ribbon_Renderer.SetPosition(0, model.LeftKnot.position);
        ribbon_Renderer.SetPosition(1, model.Projectile.position);
        ribbon_Renderer.SetPosition(2, model.RightKnot.position);

    }



    public void OnToggleCatapult(bool val)
    { 
        m_Renderer.enabled = val;
        ribbon_Renderer.enabled = val;
        //--------------------------------
        controller.Model.Projectile.gameObject.SetActive(val);
    }


    #region Trajectory


    

    public void UpdateLineRenderer(int maxPoints, (int ind, Vector3 position) PointPos)
    {
        _lr.positionCount = maxPoints;
        _lr.SetPosition(PointPos.ind, PointPos.position);
    }

    private void ToggleTrajectory(bool val)
    {
        _lr.enabled = val;
    }

    #endregion

    public class ViewEventSystem
    {
        private event Action runtime;
        
        public void AddListener(Action action) => runtime += action;
        public void RemoveListener(Action action) => runtime -= action; 
        public void InvokeListener() => runtime?.Invoke();  

    }

    public IEnumerator NextProjectileLaunchTimeout()
    {
        controller.Model.Projectile.GetComponent<MeshRenderer>().enabled = false;
        controller.Model.Projectile.GetComponent<ProjectileGrabInteractable>().enabled = false;
        yield return new WaitForSeconds(1f);
        controller.Model.Projectile.GetComponent<MeshRenderer>().enabled = true;
        controller.Model.Projectile.GetComponent<ProjectileGrabInteractable>().enabled = true;
    }

    

}
