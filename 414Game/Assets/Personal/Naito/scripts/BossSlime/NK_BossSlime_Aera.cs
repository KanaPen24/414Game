using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossSlime_Aera : MonoBehaviour
{
    private bool BossBattleFlag;
    [SerializeField] private GameObject m_BossSlime;
    private bool m_One;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player" && !m_One)
        {
            m_One = true;
            BossBattleFlag = true;
            m_BossSlime.SetActive(true);
            IS_AudioManager.instance.AllStopBGM();
            IS_AudioManager.instance.PlayBGM(BGMType.BGM_BOSS);
        }
    }


    public bool GetSetBattleFlag
    {
        get { return BossBattleFlag; }
        set { BossBattleFlag = value; }
    }
}
