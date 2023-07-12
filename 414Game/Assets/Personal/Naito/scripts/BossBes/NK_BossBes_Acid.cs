using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossBes_Acid : NK_BossBesStrategy
{
    [SerializeField] private Bossbes m_BossSlime;
    [SerializeField] private GameObject m_Acid1;
    [SerializeField] private GameObject m_Acid2;
    [SerializeField] private GameObject m_Acid3;
    [SerializeField] private GameObject m_AcidPos;

    public override void UpdateStrategy()
    {
        Instantiate(m_Acid1, m_AcidPos.transform.position, Quaternion.identity);
        Instantiate(m_Acid2, m_AcidPos.transform.position, Quaternion.identity);
        Instantiate(m_Acid3, m_AcidPos.transform.position, Quaternion.identity);
        m_BossSlime.GetSetBossBesState = BossbesState.BossBesWait;
        m_BossSlime.GetSetSAnimFlag = false;
    }
}
