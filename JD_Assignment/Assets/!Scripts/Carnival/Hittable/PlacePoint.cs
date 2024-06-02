using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacePoint : MonoBehaviour
{

    [SerializeField] private ParticleSystem _explosion;


    private void Start()
    {
        _explosion = this.GetComponentInChildren<ParticleSystem>();
    }


    public void PlayParticleSystem()
    {   
        _explosion.Play();
    }



}
