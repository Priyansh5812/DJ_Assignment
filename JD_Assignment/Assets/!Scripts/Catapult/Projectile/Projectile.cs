using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private IPoolObject<Projectile> pooler = null;

    public void SetPooler(IPoolObject<Projectile> pooler)
    { 
        this.pooler = pooler;
    }


    private void OnCollisionEnter(Collision collision)
    {
        IHittable _ref = null;

        if (collision.transform.TryGetComponent<IHittable>(out _ref))
        {
            _ref.ExecuteHit();
        }

        if (pooler == null)
            Debug.LogError("Pooler not set. Consider Revising");
        else
            pooler.EnqueueObject(this);

    }


}
