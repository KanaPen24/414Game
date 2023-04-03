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
    [SerializeField] private IS_Player Player;          //プレイヤーの情報
    [SerializeField] private YK_CursolEvent CursolEvent;//カーソルイベント

    private bool m_bParticleFlg;                        //パーティクルエフェクト用のフラグ

    // Start is called before the first frame update
    void Start()
    {
        ParticleStop();
        m_bParticleFlg = false;
    }

    private void Update()
    {
        //プレイヤーの向き比較
        if(Player.GetSetPlayerDir == PlayerDir.Left)
        {
            particlePL.transform.position = Player.transform.position + new Vector3(-0.5f, 1.0f, 0.0f);
        }
        else if(Player.GetSetPlayerDir == PlayerDir.Right)
        {
            particlePL.transform.position = Player.transform.position + new Vector3(0.5f, 1.0f, 0.0f);
        }

        if(CursolEvent.GetSetUIExist && !m_bParticleFlg)
        {
            particleUI.GetComponent<Transform>().position = CursolEvent.GetSetCurrentUI.GetSetPos;
            Hand.transform.position = CursolEvent.GetSetCurrentUI.GetSetPos;
        }
    }

    // 1. 再生
    public void ParticlePlay()
    {
        // プレイしてなかったら再生する
        if (!m_bParticleFlg)
        {
            Hand.GetComponent<Animator>().SetBool("Hand", true);
            particleUI.Play();
            particlePL.Play();
            PortalObjUI.SetActive(true);
            PortalObjPL.SetActive(true);
            m_bParticleFlg = true;
            Debug.Log(Hand.transform.position);
        }
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
        m_bParticleFlg = false;
    }

    //アニメーション動かす
    public void AnimationPlay()
    {
        // アニメーターで動かす
        //
    }
}
