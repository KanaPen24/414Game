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

    [SerializeField] private ParticleSystem particleUI; //UI用エフェクトオブジェクト
    [SerializeField] private ParticleSystem particlePL; //プレイヤー用エフェクトオブジェクト
    [SerializeField] private GameObject PortalObjUI;    //UIの歪みのオブジェクト
    [SerializeField] private GameObject PortalObjPL;    //プレイヤーの歪みのオブジェクト
    [SerializeField] private YK_HPBarVisible m_HpVisible;        // PlayerのHp表示管理
    [SerializeField] private YK_HPBar HPBar;            //HPバー
    [SerializeField] private YK_SkillIcon SkillIcon;    //スキルアイコン
    [SerializeField] private IS_Player Player;          //プレイヤーの情報

    private bool m_bParticleFlg;                        //パーティクルエフェクト用のフラグ

    // Start is called before the first frame update
    void Start()
    {
        particleUI.GetComponent<Transform>().position = HPBar.GetSetPos;
        particleUI.transform.localScale = new Vector3(2, 2, 0);
        //ParticleStop();
        m_bParticleFlg = false;
    }

    private void Update()
    {
        m_bParticleFlg = m_HpVisible.GetSetVisible;

        //プレイヤーの向き比較
        if(Player.GetSetPlayerDir<=0.0f)
        {
            particlePL.transform.position = Player.transform.position + new Vector3(-0.5f, 1.0f, 0.0f);
        }
        else
        {
            particlePL.transform.position = Player.transform.position + new Vector3(0.5f, 1.0f, 0.0f);
        }

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
        particleUI.Play();
        particlePL.Play();
        PortalObjUI.SetActive(true);
        PortalObjPL.SetActive(true);
    }

    // 2. 一時停止
    private void ParticlePause()
    {
        particleUI.Pause();
    }

    // 3. 停止
    private void ParticleStop()
    {
        particleUI.Stop();
        particlePL.Stop();
        PortalObjUI.SetActive(false);
        PortalObjPL.SetActive(false);
    }

    //アニメーション動かす
    public void AnimationPlay()
    {
        // アニメーターで動かす
        //GetComponent<Animator>().SetTrigger("Hand");
    }
}
