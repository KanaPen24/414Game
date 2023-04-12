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
    [SerializeField] private int m_nSummonTime;//召喚間隔
    [SerializeField] private GameObject m_SummonPos;
    [SerializeField] private float m_fAttackRange;//間合い
    [SerializeField] private IS_Player m_Player;//Player
    //フラグ管理
    private bool m_bSummonFlag;//召喚してよいか
    public void Update()
    {
        // =========
        // 状態遷移
        // =========
        // 「召喚 → 攻撃」
        //攻撃範囲に入ったとき、攻撃行動へ切り替える
        if (m_Player.transform.position.x - m_fAttackRange <= this.transform.position.x &&
            m_Player.transform.position.x + m_fAttackRange >= this.transform.position.x)
        {
            m_BossSlime.GetSetBossSlimeState = BossSlimeState.BossSlimeMartial;
        }
    }

    public override void UpdateStrategy()
    {
        if(!m_bSummonFlag)
        {
            StartCoroutine(Summon());
        }
    }

    private IEnumerator Summon()
    {
        m_bSummonFlag = true;
        yield return new WaitForSeconds(m_nSummonTime);
        Instantiate(m_KidsSlime, m_SummonPos.transform.position, Quaternion.identity);
        m_bSummonFlag = false;
    }
}
