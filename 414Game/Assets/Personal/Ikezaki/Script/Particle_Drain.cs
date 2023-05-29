using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Drain : MonoBehaviour
{
    ParticleSystem psys;
    ParticleSystem.Particle[] m_Particles;

    public float point = 100f;
    public float power = 1f;

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        psys = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Particles = new ParticleSystem.Particle[psys.main.maxParticles];

        int AliveParticle = psys.GetParticles(m_Particles);

        for(int i = 0;i < AliveParticle; i++)
        {
            var velocity = psys.transform.TransformPoint(m_Particles[i].velocity);
            var position = psys.transform.TransformPoint(m_Particles[i].position);

            var period = m_Particles[i].remainingLifetime * 0.9f;

            // ターゲットとの差
            var diff = target.TransformPoint(target.position) - position;
            Vector3 accel = (diff - velocity * period) * 2.0f / (period * period);

            // 一定以上の加速度で追尾を弱める
            if(accel.magnitude>point)
            {
                accel = accel.normalized * point;
            }

            // 速度計算
            velocity += accel * Time.deltaTime * power;
            m_Particles[i].velocity = psys.transform.InverseTransformPoint(velocity);
        }

        psys.SetParticles(m_Particles, AliveParticle);
        
    }
}
