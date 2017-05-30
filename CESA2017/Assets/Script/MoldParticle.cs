using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoldParticle : MonoBehaviour {

    [SerializeField,Header("パーティクルシステム")]
    ParticleSystem[] particle;

    /// <summary>
    /// 渡された値に応じたパーティクルを再生
    /// </summary>
    /// <param name="nValue">再生するパーティクル。0 = Bad, 2 = Best </param>
    /// <returns></returns>
    public void PlayParticle(int nValue)
    {
        if (nValue >= 0 && nValue <= particle.Length)
        {
            particle[nValue].Play();
        }
    }
}
