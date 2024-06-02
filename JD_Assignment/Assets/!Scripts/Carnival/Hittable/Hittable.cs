using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Hittable : MonoBehaviour
{   


    [SerializeField]private PlacePoint holder;
    void Start()
    {
        holder = GetComponentInParent<PlacePoint>();

    }

    // Update is called once per frame
    void Update()
    {
    
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Trigger"))
        {
            Transform repeatPoint = other.transform.GetChild(0).transform;
            holder.transform.SetPositionAndRotation(repeatPoint.position, repeatPoint.rotation);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
/*        Rigidbody rb = this.GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.freezeRotation = false;
        rb.useGravity = true;*/
        
    }

}
