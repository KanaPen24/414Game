using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossMole_Wait : NK_BossMoleStrategy
{
    [SerializeField] private NK_BossMole m_Mole;
    [SerializeField] private float m_fAttackTime;
    private float m_fCnt;

    public override void UpdateStrategy()
    {
        m_fCnt += Time.deltaTime;
        if(m_fCnt>m_fAttackTime)
        {
            m_fCnt = 0.0f;
        }
    }
}
