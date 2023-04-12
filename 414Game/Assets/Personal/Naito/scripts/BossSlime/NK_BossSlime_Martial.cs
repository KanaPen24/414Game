using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossSlime_Martial : NK_BossSlimeStrategy
{
    [SerializeField] private NK_BossSlime m_BossSlime;//NK_BossSlimeをアタッチする
    [SerializeField] private GameObject m_MartialArts;//攻撃の当たり判定
    [SerializeField] private int m_nMartialArtsTime;//攻撃間隔
    [SerializeField] private GameObject m_MartialArtsPos;//攻撃発生場所
    [SerializeField] private float m_fAttackRange;//間合い
    [SerializeField] private IS_Player m_Player;//Player
    //フラグ管理
    private bool m_bMartialArtsFlag;//攻撃してよいか

    public void Update()
    {
        // =========
        // 状態遷移
        // =========
        // 「攻撃 → 召喚」
        //攻撃範囲より外に出た時、召喚行動へ切り替える
        if (m_Player.transform.position.x - m_fAttackRange >= this.transform.position.x && 
            m_Player.transform.position.x + m_fAttackRange <= this.transform.position.x)
        {
            m_BossSlime.GetSetBossSlimeState = BossSlimeState.BossSlimeSummon;
        }
    }

    public override void UpdateStrategy()
    {
        if (!m_bMartialArtsFlag)
        {
            StartCoroutine(MartialArts());
        }
    }

    private IEnumerator MartialArts()
    {
        m_bMartialArtsFlag = true;
        yield return new WaitForSeconds(m_nMartialArtsTime);
        Instantiate(m_MartialArts, m_MartialArtsPos.transform.position, Quaternion.identity);
        m_bMartialArtsFlag = false;
    }
}
