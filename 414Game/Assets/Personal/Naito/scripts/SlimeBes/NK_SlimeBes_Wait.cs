using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_SlimeBes_Wait : NK_SlimeBesStrategy
{
    [SerializeField] private NK_SlimeBes m_Bes;
    [SerializeField] private float m_MoveTime;
    private float m_fCnt;

    public override void UpdateStrategy()
    {
        m_fCnt += Time.deltaTime;
        if(m_fCnt>m_MoveTime)
        {
            m_fCnt = 0.0f;
            
        }
    }
}
