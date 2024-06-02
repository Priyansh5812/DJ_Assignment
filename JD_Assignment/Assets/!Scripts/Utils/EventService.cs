using System;



public class ProjectileEvent
{
	
	private static ProjectileEvent _service;
	public static ProjectileEvent Service
	{
		get
		{
			if(_service == null)
				_service = new ProjectileEvent();
			
			return _service;
		}
		
	}
	
    private ProjectileEvent()
	{
		onGrabProjectile = new();
		onReleaseProjectile = new();
	}
	
	
	//---------- Events----------
	
	public EventController<bool> onGrabProjectile;
	public EventController<bool> onReleaseProjectile;

    


}




#region EventController (Primitives)

public class EventController
{
    public event Action baseEvent;
    public void AddListener(Action Listener) => baseEvent += Listener;
    public void RemoveListener(Action Listerner) => baseEvent -= Listerner;
    public void InvokeEvent() => baseEvent?.Invoke();

    public void ClearListeners()
    {
        foreach (var i in baseEvent.GetInvocationList())
        {
            baseEvent -= i as Action;
        }
    }

}


public class EventController<T>
{
    public event Action<T> baseEvent;
    public void AddListener(Action<T> Listener) => baseEvent += Listener;
    public void RemoveListener(Action<T> Listerner) => baseEvent -= Listerner;
    public void InvokeEvent(T val) => baseEvent?.Invoke(val);
    public void ClearListeners()
    {
        foreach (var i in baseEvent.GetInvocationList())
        {
            baseEvent -= i as Action<T>;
        }
    }


}

#endregion EventController