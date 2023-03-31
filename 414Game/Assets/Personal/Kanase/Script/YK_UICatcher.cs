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
    [SerializeField] private GameObject Hand;           //手のオブジェクト
    [SerializeField] private YK_HPBarVisible m_HpVisible;        // PlayerのHp表示管理
    [SerializeField] private YK_HPBar HPBar;            //HPバー
    [SerializeField] private YK_SkillIcon SkillIcon;    //スキルアイコン
    [SerializeField] private IS_Player Player;          //プレイヤーの情報
    [SerializeField] private YK_CursolEvent CursolEvent;          //カーソルイベント

    private bool m_bParticleFlg;                        //パーティクルエフェクト用のフラグ

    // Start is called before the first frame update
    void Start()
    {
        particleUI.GetComponent<Transform>().position = HPBar.GetSetPos;
        Hand.transform.position = HPBar.GetSetPos;
        ParticleStop();
        m_bParticleFlg = false;
    }

    private void Update()
    {
        //プレイヤーの向き比較
        if(Player.GetSetPlayerDir<=0.0f)
        {
            particlePL.transform.position = Player.transform.position + new Vector3(-0.5f, 1.0f, 0.0f);
        }
        else
        {
            particlePL.transform.position = Player.transform.position + new Vector3(0.5f, 1.0f, 0.0f);
        }

        //どのUIを選んでるかで引っ張ってくる座標を変える
        switch (CursolEvent.GetUINumber())
        {
            case 0:
                particleUI.GetComponent<Transform>().position = HPBar.GetSetPos;
                Hand.transform.position = HPBar.GetSetPos;
                break;
            case 1:
                particleUI.GetComponent<Transform>().position = SkillIcon.GetPos(0);
                Hand.transform.position = SkillIcon.GetPos(0);
                break;
            case 2:
                particleUI.GetComponent<Transform>().position = SkillIcon.GetPos(1);
                Hand.transform.position = SkillIcon.GetPos(1);
                break;
            case 3:
                particleUI.GetComponent<Transform>().position = SkillIcon.GetPos(2);
                Hand.transform.position = SkillIcon.GetPos(2);
                break;
            case 4:
                particleUI.GetComponent<Transform>().position = SkillIcon.GetPos(3);
                Hand.transform.position = SkillIcon.GetPos(3);
                break;
            case 5:
                particleUI.GetComponent<Transform>().position = SkillIcon.GetPos(4);
                Hand.transform.position = SkillIcon.GetPos(4);
                break;
        }
    }

    // 1. 再生
    public void ParticlePlay()
    {
        Hand.GetComponent<Animator>().SetBool("Hand", true);
        particleUI.Play();
        particlePL.Play();
        PortalObjUI.SetActive(true);
        PortalObjPL.SetActive(true);
    }

    // 2. 一時停止
    public void ParticlePause()
    {
        particleUI.Pause();
    }

    // 3. 停止
    public void ParticleStop()
    {
        Hand.GetComponent<Animator>().SetBool("Hand", false);
        particleUI.Stop();
        particlePL.Stop();
        PortalObjUI.SetActive(false);
        PortalObjPL.SetActive(false);
    }

    //アニメーション動かす
    public void AnimationPlay()
    {
        // アニメーターで動かす
        //
    }
}
