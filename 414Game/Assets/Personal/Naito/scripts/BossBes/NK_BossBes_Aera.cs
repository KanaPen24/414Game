using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossBes_Aera : MonoBehaviour
{
    private bool BossBattleFlag;
    [SerializeField] private IS_Player m_Player;
    [SerializeField] private GameObject m_BossSlime;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject==m_Player.gameObject)
        {
            BossBattleFlag = true;
            m_BossSlime.SetActive(true);
        }
    }


    public bool GetSetBattleFlag
    {
        get { return BossBattleFlag; }
        set { BossBattleFlag = value; }
    }
}
