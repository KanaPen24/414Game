using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_SlimeBes_Wait : NK_SlimeBesStrategy
{
    [SerializeField] private NK_SlimeBes m_Bes;
    [SerializeField] private float m_MoveTime;
    private float m_fCnt;
    private int m_AcidCnt;

    public override void UpdateStrategy()
    {
        m_fCnt += Time.deltaTime;
        if (m_fCnt>m_MoveTime)
        {
            m_fCnt = 0.0f;
            if (m_AcidCnt < 5)
            {
                m_AcidCnt += 1;
                m_Bes.GetSetMoveAnimFlag = true;
                Invoke("MFlagChange", 0.45f);
                m_Bes.GetSetBesState = SlimeBesState.BesCrawl;
            }
            else
            {
                m_AcidCnt = 0;
                m_Bes.GetSetAcidAnimFlag = true;
                Invoke("AFlagChange", 0.5f);
                m_Bes.GetSetBesState = SlimeBesState.BesAcid;
            }
        }
    }

    private void MFlagChange()
    {
        m_Bes.GetSetMoveAnimFlag = false;
    }

    private void AFlagChange()
    {
        m_Bes.GetSetAcidAnimFlag = false;
    }
}
