using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] private Material _particleMaterial;
    private ParticleSystem _particle => GetComponent<ParticleSystem>();

    public void PlayParticles()
    {
        _particle.Play();
    }

    public void ChangeColor(Color color)
    {
        _particleMaterial.color = color;    
    }
}
