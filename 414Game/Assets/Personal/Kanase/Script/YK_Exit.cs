/**
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


public class YK_Exit : YK_UI
{
    [SerializeField] private Image Exit;    
    [SerializeField] private Vector3 m_MinScale = new Vector3(0.5f, 0.5f, 0.5f); // 最小サイズ
    [SerializeField] private float m_fDelTime = 0.5f; // 減算していく時間
    private bool m_bVisibleRetry = false;
    // Start is called before the first frame update
    void Start()
    {
        m_eUIType = UIType.Exit; //UIのタイプ設定
        m_eFadeState = FadeState.FadeNone;
        //UIが動くようならUpdateにかかなかん
        GetSetPos = Exit.GetComponent<RectTransform>().anchoredPosition;
        //スケール取得
        GetSetScale = Exit.transform.localScale;
    }

    public override void UIFadeOUT()
    {
        m_eFadeState = FadeState.FadeOUT;
        // 1秒で後X,Y方向を0.5倍に変更
        Exit.transform.DOScale(m_MinScale, m_fDelTime);
        // 1秒でテクスチャをフェードアウト
        Exit.DOFade(0f, m_fDelTime).OnComplete(() =>
        {
            //フェード処理終了時に呼ばれる
            GetSetFadeState = FadeState.FadeNone;
            ExitPlay();
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
        get { return m_bVisibleRetry; }
        set { m_bVisibleRetry = value; }
    }

    public void ExitPlay()
    {
        //  終了処理
        Application.Quit();             //ゲーム終了処理
    }

}