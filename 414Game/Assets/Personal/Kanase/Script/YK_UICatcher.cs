/**
 * @file   YK_UICacher.cs
 * @brief  UIをとるための関数たち
 * @author 吉田叶聖
 * @date   2023/03/18
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YK_UICatcher : MonoBehaviour
{

    [SerializeField] private ParticleSystem particle;   //エフェクトオブジェクト
    [SerializeField] private GameObject PortalObj;      //歪みのオブジェクト
    [SerializeField] private YK_HPBarVisible m_HpVisible;        // PlayerのHp表示管理
    [SerializeField] private YK_HPBar HPBar;            //HPバー
    [SerializeField] private YK_SkillIcon SkillIcon;    //スキルアイコン

    private bool m_bParticleFlg;                        //パーティクルエフェクト用のフラグ

    // Start is called before the first frame update
    void Start()
    {
        particle.GetComponent<Transform>().position = HPBar.GetSetPos;
        particle.transform.localScale = new Vector3(2, 2, 0);
        ParticleStop();
        m_bParticleFlg = false;
    }

    private void Update()
    {
        m_bParticleFlg = m_HpVisible.GetSetVisible;
        if (!m_bParticleFlg)
        {
            ParticlePlay();
        }
        else
        {
            ParticleStop();
        }
        
        
        
        //どのUIを選んでるかで引っ張ってくる座標を変える
        //    switch (UI.GetSetUIType)
        //{
        //    case UIType.HPBar:
        //        particle.GetComponent<Transform>().position = UI.GetSetPos;
        //        break;
        //    case UIType.SkillIcon:
        //        particle.GetComponent<Transform>().position = SkillIcon.GetPos(0);
        //        break;
        //}
    }

    // 1. 再生
    private void ParticlePlay()
    {
        particle.Play();
        PortalObj.SetActive(true);
    }

    // 2. 一時停止
    private void ParticlePause()
    {
        particle.Pause();
    }

    // 3. 停止
    private void ParticleStop()
    {
        particle.Stop();
        PortalObj.SetActive(false);
    }

    //アニメーション動かす
    public void AnimationPlay()
    {
        // アニメーターで動かす
        //GetComponent<Animator>().SetTrigger("Hand");
    }
}
