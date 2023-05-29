using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_BossBes_Fall : NK_BossBesStrategy
{
    //ボスすらいむをアタッチ
    [SerializeField] private NK_BossBes m_BossSlime;
    [SerializeField] private float m_FloorPos;
    [SerializeField] private float m_FallPow;
    [SerializeField] private GameObject m_FallEffect;
    [SerializeField] private YK_TargetCamera m_Shake;
    [SerializeField] private float m_ShakePow;

    public override void UpdateStrategy()
    {
        m_BossSlime.m_BBMoveValue = new Vector3(0.0f, 0.0f, 0.0f);
        if(this.gameObject.transform.position.y>m_FloorPos)
        {
            m_BossSlime.m_BBMoveValue.y -= m_FallPow;
        }
        else
        {
            m_Shake.GetShakeFloat(m_ShakePow);
            // SE再生
            IS_AudioManager.instance.PlaySE(SEType.SE_SlimeFall);
            m_FallEffect.SetActive(false);
            m_BossSlime.GetSetBossBesState = BossBesState.BossBesWait;
        }
    }

}
