using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossSlime_Acid : NK_BossSlimeStrategy
{
    [SerializeField] private NK_BossSlime m_BossSlime;
    [SerializeField] private GameObject m_Acid1;
    [SerializeField] private GameObject m_Acid2;
    [SerializeField] private GameObject m_Acid3;
    [SerializeField] private GameObject m_SummonPos;
    [SerializeField] private float m_AcidTime;
    private float m_Cnt;

    
}
