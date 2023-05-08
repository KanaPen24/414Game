using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_Bat_Wait : NK_BatStrategy
{
    [SerializeField] private NK_Bat m_Bat;
    [SerializeField] private float m_MoveTime;
    private float m_fCnt;
    public override void UpdateStrategy()
    {
        m_fCnt += Time.deltaTime;
        if (m_fCnt > m_MoveTime)
        {
            m_fCnt = 0.0f;
            m_Bat.GetSetBatState = BatState.BatMove;
        }
    }
}
