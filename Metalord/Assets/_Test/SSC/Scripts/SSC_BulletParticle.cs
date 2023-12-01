using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSC_BulletParticle : MonoBehaviour
{
    // 총알을 발사하는 객체
    [SerializeField] private ParticleSystem particleLauncher;

    // 발사된 총알 객체가 벽에 충돌되고 호출될 객체
    [SerializeField] private ParticleSystem splatterParticle;

    [SerializeField] private Gradient randomParticecolor;

    // 충돌 이벤트들을 저장할 리스트
    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();


    // 파티클의 충돌 이벤트
    // 파티클 충돌 감지레이어는 인스펙터창에서 구분 지어놨음
    private void OnParticleCollision(GameObject other)
    {
        // 할당된 파티클의 이벤트 정보를 리스트에 담는 메소드
        ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvents);
        
        for(int i = 0; i < collisionEvents.Count; i++)
        {
            EmitSpaltter(collisionEvents[i]);
        }
    }

    void EmitSpaltter(ParticleCollisionEvent particleEvent)
    {
        ParticleSystem.MainModule splatteMain = splatterParticle.main;
        
        // 이벤트가 발생한 위치 정보
        // 충돌하고난 이후 발생할 파티클의 위치를 잡아준다.
        splatterParticle.transform.position = particleEvent.intersection;

        // 파티클의 로테이션값은 이벤트 발생지점의 정규화 방향
        splatterParticle.transform.rotation = Quaternion.LookRotation(particleEvent.normal);

        splatteMain.startColor = randomParticecolor.Evaluate(Random.Range(0, 1));

        // 해당하는 파티클을 매개인수 만큼 방출하는 메소드.
        splatterParticle.Emit(1);
    }
}
