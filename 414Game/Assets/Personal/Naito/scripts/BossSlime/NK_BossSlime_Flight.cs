using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossSlime_Flight : NK_BossSlimeStrategy
{
    [SerializeField] private NK_BossSlime m_BossSlime;
    [SerializeField] private float m_fMovePow;
    [SerializeField] private float m_Reng;
    private bool m_FallFlag;
    private float m_FlightCnt;
    private float m_FallCnt;
    [SerializeField] private float m_FallTime;
    [SerializeField] private float m_FlightTime;
    [SerializeField] private GameObject m_Danger;
    [SerializeField] private GameObject m_FallEffect;


    public override void UpdateStrategy()
    {
        m_BossSlime.m_BSMoveValue = new Vector3(0.0f, 0.0f, 0.0f);
        m_FlightCnt += Time.deltaTime;
        if ((this.gameObject.transform.position.x > m_BossSlime.m_BSPlayer.transform.position.x - m_Reng) &&
            (this.gameObject.transform.position.x < m_BossSlime.m_BSPlayer.transform.position.x + m_Reng) &&
            m_FlightCnt > m_FlightTime)
        {
            m_FallFlag = true;
        }
        else
        {
            if (!m_FallFlag)
            {
                if (m_BossSlime.GetSetBossSlimeDir == BossSlimeDir.Left)
                {
                    m_BossSlime.m_BSMoveValue.x -= m_fMovePow;
                }
                if (m_BossSlime.GetSetBossSlimeDir == BossSlimeDir.Right)
                {
                    m_BossSlime.m_BSMoveValue.x += m_fMovePow;
                }
            }
        }

        if(m_FallFlag)
        {
            m_FallCnt += Time.deltaTime;
            if (m_FallCnt>m_FallTime)
            {
                m_FallCnt = 0;
                m_FlightTime = 0;
                m_Danger.SetActive(false);
                m_FallFlag = false;
                // エフェクト再生
                //ParticleSystem Effect = Instantiate(m_FallEffect);
                //Effect.Play();
                //Effect.transform.position = new Vector3(this.transform.position.x,
                //    this.transform.position.y - 0.45f, this.transform.position.z);
                //Destroy(Effect.gameObject, 5.0f);
                m_FallEffect.SetActive(true);
                m_BossSlime.GetSetBossSlimeState = BossSlimeState.BossSlimeFall;
            }
        }
    }
}
