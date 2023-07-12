using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossBes_Wait : NK_BossBesStrategy
{
    [SerializeField] private Bossbes m_BossSlime;//NK_BossSlimeをアタッチする
    [SerializeField] private float m_fAttackTime;//攻撃間隔
    [SerializeField] private float m_fAttackRange;//間合い
    [SerializeField] private IS_Player m_Player;//Player
    private float m_fCnt;
    private bool m_SPAttackFlag;
    private Animator anim;
    private int m_AcidCnt;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void UpdateStrategy()
    {
        m_fCnt += Time.deltaTime;
        if (m_fCnt > m_fAttackTime)
        {
            m_fCnt = 0.0f;
            if (m_BossSlime.GetSetHp <= 25 && !m_SPAttackFlag)
            {
                m_SPAttackFlag = true;
                m_BossSlime.GetSetBossBesState = BossbesState.BossBesUp;
            }
            else if(m_AcidCnt>3)
            {
                m_BossSlime.GetSetSAnimFlag = true;
                m_BossSlime.GetSetBossBesState = BossbesState.BossBesAcid;
                m_AcidCnt = 0;
            }
            else if ((this.transform.position.x - m_fAttackRange >= m_Player.transform.position.x) ||
                (this.transform.position.x + m_fAttackRange <= m_Player.transform.position.x))
            {
                m_BossSlime.GetSetSAnimFlag = true;
                m_BossSlime.GetSetBossBesState = BossbesState.BossBesSummon;
                m_AcidCnt++;
            }
            else
            {
                m_BossSlime.GetSetMAnimFlag = true;
                m_BossSlime.GetSetBossBesState = BossbesState.BossBesMartial;
                m_AcidCnt++;
            }
        }
    }
}
