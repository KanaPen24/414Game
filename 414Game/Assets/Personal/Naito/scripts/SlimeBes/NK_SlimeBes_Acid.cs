using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_SlimeBes_Acid : NK_SlimeBesStrategy
{
    [SerializeField] private GameObject m_gAcid;
    [SerializeField] private GameObject m_AcidPos;
    [SerializeField] private bes m_Bes;

    public override void UpdateStrategy()
    {
        Instantiate(m_gAcid, m_AcidPos.transform.position, Quaternion.identity);
        m_Bes.GetSetBesState = BesState.BesWait;
    }
}
