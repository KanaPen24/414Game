using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossBes_Flight : NK_BossBesStrategy
{
    [SerializeField] private Bossbes m_BossSlime;
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
        m_BossSlime.m_BBMoveValue = new Vector3(0.0f, 0.0f, 0.0f);
        m_FlightCnt += Time.deltaTime;
        if ((this.gameObject.transform.position.x > m_BossSlime.m_BBPlayer.transform.position.x - m_Reng) &&
            (this.gameObject.transform.position.x < m_BossSlime.m_BBPlayer.transform.position.x + m_Reng) &&
            m_FlightCnt > m_FlightTime)
        {
            m_FallFlag = true;
        }
        else
        {
            if (!m_FallFlag)
            {
                if (m_BossSlime.GetSetEnemyDir == EnemyDir.Left)
                {
                    m_BossSlime.m_BBMoveValue.x -= m_fMovePow;
                }
                if (m_BossSlime.GetSetEnemyDir == EnemyDir.Right)
                {
                    m_BossSlime.m_BBMoveValue.x += m_fMovePow;
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
                m_BossSlime.GetSetBossBesState = BossbesState.BossBesFall;
            }
        }
    }
}
