/**
 * @file   IS_GoalEffect.cs
 * @brief  ゴールエフェクトのクラス
 * @author IharaShota
 * @date   2023/04/12
 * @Update 2023/04/12 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IS_GoalEffect : MonoBehaviour
{
    [System.Serializable]
    private class CEffectParam
    {
        public Vector3 m_vLocalPos;
        public Vector3 m_vLocalRot;
        public Vector3 m_vLocalScale;
    }

    [SerializeField] private IS_Player m_Player;              // Player
    [SerializeField] private ParticleSystem confettiEffect;   // 紙吹雪エフェクト(倒されたとき発生)
    [SerializeField] private List<CEffectParam> m_effectParam;// エフェクトパラメータ

    /**
     * @fn
     * エフェクト再生処理
     * @brief  
     * @detail 
     */
    public void StartEffect()
    {
        for(int i = 0,size = m_effectParam.Count; i < size; ++i)
        {
            ParticleSystem effect = Instantiate(confettiEffect);
            effect.Play();
            effect.transform.position =
                new Vector3(m_Player.transform.position.x, 0f, m_Player.transform.position.z) + 
                m_effectParam[i].m_vLocalPos;
            effect.transform.rotation = Quaternion.Euler(m_effectParam[i].m_vLocalRot);
            effect.transform.localScale = m_effectParam[i].m_vLocalScale;

            Debug.Log(m_Player.transform.position);

            Destroy(effect.gameObject, 5.0f); // 5秒後に消える
        }
    }
}
