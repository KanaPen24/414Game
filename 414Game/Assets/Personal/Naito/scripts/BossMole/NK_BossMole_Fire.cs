using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossMole_Fire : NK_BossMoleStrategy
{
    [SerializeField] private NK_BossMole m_Mole;
    [SerializeField] private GameObject m_FirePos;
    [SerializeField] private GameObject m_Fire;

    public override void UpdateStrategy()
    {
        Instantiate(m_Fire, m_FirePos.transform.position, Quaternion.identity);
        m_Mole.GetSetBossMoleState = BossMoleState.MoleWait;
    }
}
