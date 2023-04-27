/**
 * @file   IS_BossSlime_Summon.cs
 * @brief  BossSlimeの召喚行動
 * @author NaitoKoki
 * @date   2023/04/10
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossSlime_Summon : NK_BossSlimeStrategy
{
    [SerializeField] private NK_BossSlime m_BossSlime;//NK_BossSlimeをアタッチする
    [SerializeField] private GameObject m_KidsSlime;//召喚するすらいむ
    [SerializeField] private GameObject m_SummonPos;

    public override void UpdateStrategy()
    {
        GameObject childSlimeObj = 
        Instantiate(m_KidsSlime, m_SummonPos.transform.position, Quaternion.identity);
        childSlimeObj.SetActive(true);
        IS_AudioManager.instance.PlaySE(SEType.SE_SlimeCreate);
        // =========
        // 状態遷移
        // =========
        // 「召喚 → 待機」
        m_BossSlime.GetSetBossSlimeState = BossSlimeState.BossSlimeWait;
    }

}
