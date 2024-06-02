using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IHittable
{
    public void Re_EvaluateHittableStats();
    public void ExecuteHit();
}


public class Hittable : MonoBehaviour , IHittable
{   

    [SerializeField]private PlacePoint holder;
    [SerializeField] private Rigidbody rb;
    [Range(1f , 7.5f)]
    [SerializeField] private float disableDuration;
    private HittableStats stats;
    void Start()
    {
        holder = GetComponentInParent<PlacePoint>();
        rb = this.GetComponent<Rigidbody>();
        stats = new();
        (this as IHittable).Re_EvaluateHittableStats();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Trigger"))
        {
            Transform repeatPoint = other.transform.GetChild(0).transform;
            holder.transform.SetPositionAndRotation(repeatPoint.position, repeatPoint.rotation);
        }
    }


     void IHittable.Re_EvaluateHittableStats()
    { 
        stats.color = (Random.Range(0 , 101) < 60) ? Color.red : Color.green;

        GetComponent<MeshRenderer>().material.color = stats.color;
        
        stats.points = stats.color == Color.red ? 100 : 50;
    }

     void IHittable.ExecuteHit()
    {
        holder.PlayParticleSystem();
        StartCoroutine(OnHitCoroutine());
        ProjectileEvent.Service.onProjectileHit.InvokeEvent(stats.points);
        Debug.Log("Hit Registered");
    }

    private IEnumerator OnHitCoroutine()
    {   
        //----Disable Collision Detection-------------
        rb.detectCollisions = false;
        //----Disable Renderer------------------------
        this.GetComponent<MeshRenderer>().enabled = false;

        //---------Wait for Some Time-----------------
        yield return new WaitForSeconds(disableDuration);
        
        //--------- Re-evaluate Hittable Stat---------
        (this as IHittable).Re_EvaluateHittableStats();

        //--------- Revert all booleans---------------
        this.GetComponent<MeshRenderer>().enabled = true;
        rb.detectCollisions = true;
    }

    [System.Serializable]
    private struct HittableStats
    {
        public int points;
        public Color color;
        public float disableDuration;
    }

}
