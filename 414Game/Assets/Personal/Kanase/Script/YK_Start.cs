﻿/**
 * @file YK_Retry.cs
 * @brief RetryUIの処理
 * @author 吉田叶聖
 * @date 2023/05/02
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class YK_Start : YK_UI
{
    [SerializeField] private GameObject GameStart;
    [SerializeField] private Image StartUI;
    [SerializeField] private YK_Hand m_Hand;
    [SerializeField] private Vector3 m_MinScale = new Vector3(0.5f, 0.5f, 0.5f); // 最小サイズ
    [SerializeField] private float m_fDelTime = 0.5f; // 減算していく時間
    [SerializeField] ON_VolumeManager PostEffect; // ポストエフェクト
    private bool m_bVisibleStart = true;
    private float m_rate = 1.0f;
    private float m_fTime;
    // Start is called before the first frame update
    void Start()
    {
        m_eUIType = UIType.Start; //UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        //UIが動くようならUpdateにかかなかん
        GetSetPos = StartUI.GetComponent<RectTransform>().anchoredPosition;
        //スケール取得
        GetSetScale = StartUI.transform.localScale;
    }
    private void Update()
    {
        if (!m_bVisibleStart)
        {
            m_fTime += Time.deltaTime;
            m_rate = Mathf.Lerp(1.0f, 0.0f, m_fTime);
            PostEffect.SetBraunRate(m_rate);
            Debug.Log(m_rate);
        }
        if (GameManager.instance.GetSetGameState != GameState.GameStart && m_rate <= 0) 
            GameStart.SetActive(false);
        else
            GameStart.SetActive(true);
        
    }
    //StartUIを表示
    public override void UIFadeIN()
    {
        m_eFadeState = FadeState.FadeIN;
        // 1秒で後X,Y方向を元の大きさに変更
        StartUI.transform.DOScale(GetSetScale, 0f);
        // 1秒でテクスチャをフェードイン
        StartUI.DOFade(1f, 0f).OnComplete(() =>
        {
            GetSetFadeState = FadeState.FadeNone;
            Debug.Log("FadeIN終了");
        });
    }
    //StartUIを非表示
    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        // 1秒で後X,Y方向を0.5倍に変更
        StartUI.transform.DOScale(m_MinScale, m_fDelTime);
        // 1秒でテクスチャをフェードアウト
        StartUI.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            //フェード処理終了時に呼ばれる
            GetSetFadeState = FadeState.FadeNone;
            StartPlay();
            Debug.Log("FadeOUT終了");
        });
        
    }
    /**
 * @fn
 * 表示非表示のgetter・setter
 * @return m_bVisibleStart(bool)
 * @brief 表示非表示処理
 */
    public bool GetSetVisibleFlg
    {
        get { return m_bVisibleStart; }
        set { m_bVisibleStart = value; }
    }

    public void StartPlay()
    {
        m_bVisibleStart = false;
        //ゲームのステートをプレイ状態にする
        GameManager.instance.GetSetGameState = GameState.GamePlay;
    }

}