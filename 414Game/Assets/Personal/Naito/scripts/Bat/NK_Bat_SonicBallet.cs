using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_Bat_SonicBallet : MonoBehaviour
{
    [SerializeField] private float m_SonicSpeed;
    [SerializeField] private IS_Player m_Player;
    [SerializeField] private YK_Clock m_Clock;
    [SerializeField] private ParticleSystem m_DestroyEffect;

    private void FixedUpdate()
    {
        if(m_Clock.GetSetStopTime)
        {
            return;
        }
        this.gameObject.transform.Translate(m_SonicSpeed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == m_Player.gameObject)
        {
            m_Player.Damage(15, 1.5f);
            ParticleSystem Effect = Instantiate(m_DestroyEffect);
            Effect.Play();
            Effect.transform.position = this.transform.position;
            Destroy(Effect.gameObject, 2.0f);
            Destroy(this.gameObject);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            ParticleSystem Effect = Instantiate(m_DestroyEffect);
            Effect.Play();
            Effect.transform.position = this.transform.position;
            Destroy(Effect.gameObject, 2.0f);
            Destroy(this.gameObject);
        }
        
    }
}
