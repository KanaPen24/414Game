using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossSlime_Martial : NK_BossSlimeStrategy
{
    [SerializeField] private NK_BossSlime m_BossSlime;//NK_BossSlimeをアタッチする
    [SerializeField] private GameObject m_MartialArts;//攻撃の当たり判定
    [SerializeField] private GameObject m_MartialArtsPos;//攻撃発生場所
    private float m_Cnt;
    [SerializeField] private float m_AttackTime;

    public override void UpdateStrategy()
    {
        m_Cnt += Time.deltaTime;
        if (m_Cnt > m_AttackTime)
        {
            m_Cnt = 0.0f;
            // SE再生
            IS_AudioManager.instance.PlaySE(SEType.SE_SlimeFall);
            m_MartialArts.SetActive(true);
            //Instantiate(m_MartialArts, m_MartialArtsPos.transform.position, Quaternion.identity);
            // =========
            // 状態遷移
            // =========
            // 「攻撃 → 待機」
            m_BossSlime.GetSetMAnimFlag = false;
            m_BossSlime.GetSetBossSlimeState = BossSlimeState.BossSlimeWait;
        }
    }
}
