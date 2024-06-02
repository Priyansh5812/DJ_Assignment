using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolObject<T>
{   
    // Pooling Abstractions
    public void EnqueueObject(T obj);
    public T CreateOrDequeueObject();

}


public class ProjectilePooler : IPoolObject<Projectile>
{

    private readonly Queue<Projectile> queue;
    private Projectile projectilePrefab;
    private IPoolObject<Projectile> pooler;
    public ProjectilePooler(Projectile obj)
    { 
        queue = new Queue<Projectile>();
        this.projectilePrefab = obj;

    }

    Projectile IPoolObject<Projectile>.CreateOrDequeueObject()
    {

        Projectile obj;
        if (queue.Count == 0)
        {
            obj = Object.Instantiate(projectilePrefab);
            obj.SetPooler(this);
            
        }
        else 
        {
            obj = queue.Dequeue();
            OnDequeueOperations(ref obj);
        }



        return obj;
    }

     void IPoolObject<Projectile>.EnqueueObject(Projectile obj)
    {   
        OnEnqueueOperations(ref obj);
        queue.Enqueue(obj);

    }

    void OnEnqueueOperations(ref Projectile obj)
    {
        Rigidbody rb;
        obj.transform.TryGetComponent<Rigidbody>(out rb);
        rb.velocity = Vector3.zero;
        obj.gameObject.SetActive(false);
    }
    void OnDequeueOperations(ref Projectile obj)
    {
        Rigidbody rb;
        obj.transform.TryGetComponent<Rigidbody>(out rb);
        rb.velocity = Vector3.zero;
    }
}
