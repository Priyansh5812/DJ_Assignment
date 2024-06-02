using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultView : MonoBehaviour
{

    public Transform System { get; private set; }
    private CatapultController controller = null;
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
        OnToggleCatapult(false);
    }

    // Update is called once per frame
    void Update()
    {   
        // Runtime Invocation for Controller Logic Executions (if any)
        controller_Runtime?.InvokeListener();

        UpdateCatapultribbon_Renderer();

        PredictTrajectory();
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

    public void PredictTrajectory()
    {
        //Vector3 velocity = _trajectoryProps.direction * (_trajectoryProps.initialSpeed / _trajectoryProps.mass); // direction * speed per mass
        Vector3 velocity = controller.GetInitialDirection() * (_trajectoryProps.initialSpeed / _trajectoryProps.mass); // direction * speed per mass
        Vector3 position = controller.Model.Projectile.position;
        Vector3 nextPosition;
        float overlap;
        UpdateLineRenderer(maxPoints, (0, position)); // Setting initial pos  for line renderer
        for (int i = 1; i < maxPoints; i++)
        {
            velocity = controller.CalculateVelocity(velocity, _trajectoryProps.drag); // Adding effect of gravity and Air drag
            nextPosition = position + velocity * 0.01333f;
            overlap = Vector3.Distance(position, nextPosition) * rayOverlap;
            if (Physics.Raycast(position, velocity.normalized, out RaycastHit hit, overlap))
            {
                UpdateLineRenderer(i, (i - 1, hit.point));
                break;
            }
            position = nextPosition;
            UpdateLineRenderer(maxPoints, (i, position));
        }

    }

    public void UpdateLineRenderer(int maxPoints, (int ind, Vector3 position) PointPos)
    {
        _lr.positionCount = maxPoints;
        _lr.SetPosition(PointPos.ind, PointPos.position);
    }

    private void ToggleTrajectory()
    { 
        
    }

    #endregion

    //WILL THIS BE USED,Try not to
    public class ViewEventSystem
    {
        private event Action runtime;
        
        public void AddListener(Action action) => runtime += action;
        public void RemoveListener(Action action) => runtime -= action; 
        public void InvokeListener() => runtime?.Invoke();  

    }

    

}
