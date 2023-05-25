using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_SlimeBes_Acid : NK_BatStrategy
{
    [SerializeField] private GameObject m_gAcid;
    [SerializeField] private GameObject m_AcidPos;
    [SerializeField] private NK_SlimeBes m_Bes;

    public override void UpdateStrategy()
    {
        Instantiate(m_gAcid, m_AcidPos.transform.position, Quaternion.identity);
        m_Bes.GetSetBesState = SlimeBesState.BesWait;
    }
}
