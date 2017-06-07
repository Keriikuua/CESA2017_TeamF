using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour
{
    private ParticleSystem particle;
 
    // Use this for initialization
    void Start ()
    {
        particle = this.GetComponent<ParticleSystem> ();
 
        // ここで Particle System を停止する.
        particle.Stop ();
    }
 
    void OnTriggerEnter (Collider col)
    {
        // ここで Particle System を開始します.
        particle.Play ();
    }
}