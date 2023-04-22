using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossSlime_Wait : NK_BossSlimeStrategy
{
    [SerializeField] private NK_BossSlime m_BossSlime;//NK_BossSlimeをアタッチする
    [SerializeField] private float m_fAttackTime;//攻撃間隔
    [SerializeField] private float m_fAttackRange;//間合い
    [SerializeField] private IS_Player m_Player;//Player
    private float m_fCnt;

    public override void UpdateStrategy()
    {
        m_fCnt += Time.deltaTime;
        if (m_fCnt > m_fAttackTime)
        {
            m_fCnt = 0.0f;
            if (this.transform.position.x - m_fAttackRange >= m_Player.transform.position.x)
            {
                m_BossSlime.GetSetBossSlimeState = BossSlimeState.BossSlimeSummon;
            }
            else
            {
                m_BossSlime.GetSetBossSlimeState = BossSlimeState.BossSlimeMartial;
            }
        }
    }
}
