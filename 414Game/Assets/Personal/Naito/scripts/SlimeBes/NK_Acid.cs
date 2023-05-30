using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_Acid : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rBody;
    //横移動
    [SerializeField] private float m_fMovePower;
    //ジャンプ力
    [SerializeField] private float m_fJumpPower;
    [SerializeField] private IS_Player m_Player;
    [SerializeField] private ParticleSystem m_DestroyEffect;
    // Start is called before the first frame update
    void Start()
    {
        if(this.gameObject.transform.position.x<m_Player.gameObject.transform.position.x)
        {
            m_rBody.AddForce(transform.up * m_fJumpPower, ForceMode.Impulse);
            m_rBody.AddForce(transform.right * m_fMovePower, ForceMode.Impulse);
        }
        else
        {
            m_rBody.AddForce(transform.up * m_fJumpPower, ForceMode.Impulse);
            m_rBody.AddForce(transform.right * -m_fMovePower, ForceMode.Impulse);
        }
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
        if (collision.gameObject.CompareTag("Floor"))
        {
            ParticleSystem Effect = Instantiate(m_DestroyEffect);
            Effect.Play();
            Effect.transform.position = this.transform.position;
            Destroy(Effect.gameObject, 2.0f);
            Destroy(this.gameObject);
        }

    }
}
