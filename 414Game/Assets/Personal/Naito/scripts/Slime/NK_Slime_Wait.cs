using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_Slime_Wait : NK_SlimeStrategy
{
    [SerializeField] private NK_Slime m_Slime;//NK_Slimeをアタッチする
    [SerializeField] private float m_fAttackTime;//攻撃間隔
    private float m_fCnt;

    public override void UpdateStrategy()
    {
        m_fCnt += Time.deltaTime;
        if (m_fCnt > m_fAttackTime)
        {
            m_fCnt = 0.0f;
            m_Slime.GetSetSlimeState = SlimeState.SlimeMove;
        }
    }
}