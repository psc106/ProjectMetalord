using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSC_ParticleLauncher : MonoBehaviour
{
    // 총알을 발사하는 객체
    [SerializeField] private ParticleSystem particleLauncher;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {  
            // 제어하려하는 파티클의 속성을 담아두기.
            ParticleSystem.MainModule bulletParticle = particleLauncher.main;
            bulletParticle.startColor = Color.blue;

            particleLauncher.Emit(1);
                            
        }
    }
}
